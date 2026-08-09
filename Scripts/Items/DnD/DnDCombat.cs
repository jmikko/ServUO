using System;

namespace Server.Items
{
	/// <summary>
	/// Resolves a single attack: d20 + attack bonus vs. AC, then weapon dice + ability modifier.
	/// <para>
	/// This replaces BaseWeapon's CheckHit/ComputeDamage entirely. Everything BaseWeapon layered on
	/// top of the roll - hit chance from skills, AOS attributes, slayers, elemental damage splits,
	/// weapon abilities, durability - has no SRD counterpart and is simply gone.
	/// </para>
	/// </summary>
	public static class DnDCombat
	{
		/// <summary>Time between swings. SRD has no swing speed, so everyone attacks at one rate.</summary>
		public static readonly TimeSpan SwingDelay = TimeSpan.FromSeconds(2.0);

		/// <summary>The outcome of one attack roll, before any of it is applied to the world.</summary>
		public struct AttackResult
		{
			public int Roll;
			public bool Hit;
			public bool Critical;
			public int Damage;
		}

		/// <summary>
		/// Rolls one attack. Pure - it changes nothing, which keeps the rules testable in isolation
		/// from HP, death and packet side effects.
		/// <para>
		/// A natural 20 always hits and rolls the damage dice twice; a natural 1 always misses.
		/// Neither cares about bonuses.
		/// </para>
		/// </summary>
		public static AttackResult RollAttack(Mobile attacker, IDamageable defender, IDnDEquipment weapon)
		{
			AttackResult result = new AttackResult();

			if (attacker == null || defender == null)
			{
				return result;
			}

			bool ranged = IsRanged(weapon);
			bool finesse = weapon != null && weapon.IsFinesse;

			// Conditions decide how the die is rolled: the attacker's own impairments give it
			// disadvantage, the defender's give the attacker advantage, and one of each cancels.
			RollMode mode = DnDConditions.GetAttackRollMode(attacker, defender);

			result.Roll = CombatRules.RollD20(mode);

			// A Champion crits on 19 as well as 20; everyone else needs the 20.
			var character = attacker as IDnDCharacter;

			result.Critical = result.Roll >= ClassFeatures.GetCriticalThreshold(character);

			if (result.Roll == 1)
			{
				return result;
			}

			// Bless and Bane add or subtract a fresh die. Rolled once here and reused below, since
			// asking again would spend a one-shot modifier twice. A natural 1 or 20 has already
			// decided the outcome, so neither wastes one.
			int modifier = DnDRollModifiers.Roll(attacker, RollKind.Attack);

			if (!result.Critical &&
				result.Roll + modifier + CombatRules.GetAttackBonus(attacker, ranged, finesse, weapon as Item) <
				CombatRules.GetArmorClass(defender))
			{
				return result;
			}

			result.Hit = true;

			string dice = GetDamageDice(attacker, weapon);
			int damage = CombatRules.RollDice(dice);

			if (result.Critical)
			{
				damage += CombatRules.RollDice(dice);

				// Brutal Critical adds weapon dice beyond the usual doubling.
				int extraDice = ClassFeatures.GetExtraCriticalDice(character);

				for (int i = 0; i < extraDice; ++i)
				{
					damage += CombatRules.RollDice(dice);
				}
			}

			damage += CombatRules.GetDamageBonus(attacker, ranged, finesse, weapon as Item);

			// Sneak Attack and its kin. Rolled after the critical doubling deliberately: the SRD
			// doubles a critical's dice, and these dice are part of the attack, but doubling them
			// here as well would compound with the weapon dice already doubled above.
			damage += ClassFeatures.RollBonusDamage(character, mode);
			damage += Feat.GetDamageBonus(character, WeaponContext.For(attacker, weapon));
			damage += Server.DnDRollModifiers.Roll(attacker, Server.RollKind.Damage);
			
			if (Server.Spells.DnD.DnDEffects.HasHuntersMark(attacker, defender as Mobile))
			{
				damage += Utility.Dice(1, 6, 0);
			}

			result.Damage = Math.Max(1, damage); // a hit always does something

			return result;
		}

		/// <summary>Rolls one attack and applies it: damage, animation, combat text.</summary>
		public static TimeSpan Resolve(Mobile attacker, IDamageable defender, IDnDEquipment weapon)
		{
			if (attacker == null || defender == null)
			{
				return SwingDelay;
			}

			attacker.Direction = attacker.GetDirectionTo(defender);
			PlaySwing(attacker, IsRanged(weapon));

			// Extra Attack: one attack action, several swings. Each is rolled separately, so each
			// can miss, crit, and roll its own Sneak Attack - which is the point of it being extra
			// attacks rather than extra damage.
			int swings = 1 + ClassFeatures.GetExtraAttacks(attacker as IDnDCharacter);

			for (int i = 0; i < swings; ++i)
			{
				// A target that died to the first swing does not get hit again.
				if (defender.Deleted || !defender.Alive)
				{
					break;
				}

				AttackResult result = RollAttack(attacker, defender, weapon);

				if (!result.Hit)
				{
					// A miss is a trigger too: Riposte turns it into an opening, once per round.
					if (Engines.Classes.Features.RiposteFeature.OnMissed(defender as Mobile, attacker))
					{
						AttackResult counter = RollAttack(defender as Mobile, attacker, null);

						if (counter.Hit)
						{
							attacker.Damage(counter.Damage, defender as Mobile);
						}
					}

					Announce(attacker, defender, "misses");
					continue;
				}

				int applied = result.Damage;

				// Rage and its kin halve weapon damage.
				if (ClassFeatures.ResistsPhysicalDamage(defender as IDnDCharacter) || Server.DnDRollModifiers.HasResistance(defender as Mobile))
				{
					applied = Math.Max(1, applied / 2);
				}

				// The reaction features - Uncanny Dodge, Deflect Missiles - each decide for
				// themselves whether this hit is worth the round's one reaction.
				applied = ClassFeatures.ReduceIncomingDamage(
					defender as Mobile, defender as IDnDCharacter, applied, IsRanged(weapon));

				// A hit on someone already down costs them a death save rather than hit points.
				Mobile downed = defender as Mobile;

				if (Mobiles.DnDDeath.IsDying(downed))
				{
					Mobiles.DnDDeath.OnDamagedWhileDying(downed, result.Critical);
					Announce(attacker, defender, "strikes the fallen");
					continue;
				}

				// Damage while shaped comes off the beast.s hit points, not the character.s.
				if (!Mobiles.DnDWildShape.OnDamage(downed, applied))
				{
				defender.Damage(applied, attacker);
				}

				// Taking a hit risks dropping whatever the defender was concentrating on.
				Server.Spells.DnD.DnDConcentration.OnDamaged(defender as Mobile, applied);

				Announce(attacker, defender, result.Critical ? "critically hits" : "hits");
			}

			return SwingDelay;
		}

		private static bool IsRanged(IDnDEquipment weapon)
		{
			if (weapon == null)
			{
				return false;
			}

			return weapon.WeaponCategory == WeaponCategory.SimpleRanged ||
				   weapon.WeaponCategory == WeaponCategory.MartialRanged;
		}

		/// <summary>
		/// Monsters attack with their stat block, not with carried gear; players use the wielded
		/// weapon, falling back to an SRD unarmed strike (1 point + Str mod, so "1d1" here).
		/// </summary>
		private static string GetDamageDice(Mobile attacker, IDnDEquipment weapon)
		{
			// A shapechanged character attacks with the beast.s natural weapons, not their sword.
			string shaped = Mobiles.DnDWildShape.GetDamageDice(attacker);

			if (!String.IsNullOrEmpty(shaped))
			{
				return shaped;
			}

			IDnDCreature creature = attacker as IDnDCreature;

			if (creature != null && !String.IsNullOrEmpty(creature.DamageDiceExpression))
			{
				return creature.DamageDiceExpression;
			}

			if (weapon != null && !String.IsNullOrEmpty(weapon.DamageDiceExpression))
			{
				return weapon.DamageDiceExpression;
			}

			return "1d1";
		}

		private static void PlaySwing(Mobile attacker, bool ranged)
		{
			if (attacker.Body.IsHuman)
			{
				attacker.Animate(ranged ? 18 : 9, 7, 1, true, false, 0);
			}
		}

		/// <summary>
		/// Overhead text is the whole combat log for now - the D&amp;D message packets don't exist
		/// yet, and silent swings make live testing impossible.
		/// </summary>
		private static void Announce(Mobile attacker, IDamageable defender, string verb)
		{
			Mobile target = defender as Mobile;

			if (target == null)
			{
				return;
			}

			string text = String.Format("{0} {1} {2}", attacker.Name, verb, target.Name);

			attacker.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, text);
		}
	}

	/// <summary>
	/// The unarmed strike, installed as <see cref="Mobile.DefaultWeapon"/>. Without this every
	/// mobile with no weapon equipped - which is every monster - returns a null weapon and the
	/// engine's combat timer throws on the first swing.
	/// </summary>
	public class DnDFists : IWeapon
	{
		public static void Initialize()
		{
			Mobile.DefaultWeapon = new DnDFists();
		}

		public int MaxRange { get { return 1; } }

		public void OnBeforeSwing(Mobile attacker, IDamageable damageable)
		{ }

		public TimeSpan OnSwing(Mobile attacker, IDamageable damageable)
		{
			return DnDCombat.Resolve(attacker, damageable, null);
		}

		public void GetStatusDamage(Mobile from, out int min, out int max)
		{
			min = 1;
			max = 1;
		}

		public TimeSpan GetDelay(Mobile attacker)
		{
			return DnDCombat.SwingDelay;
		}
	}
}
