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
			result.Roll = CombatRules.RollD20(DnDConditions.GetAttackRollMode(attacker, defender));
			result.Critical = result.Roll == 20;

			if (result.Roll == 1)
			{
				return result;
			}

			// Bless and Bane add or subtract a fresh die. Rolled once here and reused below, since
			// asking again would spend a one-shot modifier twice. A natural 1 or 20 has already
			// decided the outcome, so neither wastes one.
			int modifier = DnDRollModifiers.Roll(attacker, RollKind.Attack);

			if (!result.Critical &&
				result.Roll + modifier + CombatRules.GetAttackBonus(attacker, ranged, finesse) <
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
			}

			damage += CombatRules.GetDamageBonus(attacker, ranged, finesse);

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

			AttackResult result = RollAttack(attacker, defender, weapon);

			attacker.Direction = attacker.GetDirectionTo(defender);
			PlaySwing(attacker, IsRanged(weapon));

			if (!result.Hit)
			{
				Announce(attacker, defender, "misses");
				return SwingDelay;
			}

			defender.Damage(result.Damage, attacker);

			// Taking a hit risks dropping whatever the defender was concentrating on.
			Server.Spells.DnD.DnDConcentration.OnDamaged(defender as Mobile, result.Damage);

			Announce(attacker, defender, result.Critical ? "critically hits" : "hits");

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
