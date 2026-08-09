using System;

namespace Server.Engines.Classes.Features
{
	/// <summary>
	/// The features that were blocked on a turn economy, now that there is one.
	/// <para>
	/// Each of these is phrased in the SRD as something you do in reaction to something else, and
	/// each is limited to once per round by the reaction it costs. That limit is what they are
	/// actually about - Uncanny Dodge halving one attack per round is most of Uncanny Dodge, and
	/// the ordering rules it also has are not what makes it feel like a Rogue ability.
	/// </para>
	/// <para>
	/// They spend their own reaction rather than having the caller do it, because whether a hit is
	/// worth reacting to is the feature's decision: Deflect Missiles should not burn the round's
	/// reaction on an arrow that did two damage when Uncanny Dodge might want it.
	/// </para>
	/// </summary>
	public sealed class UncannyDodgeFeature : ClassFeature
	{
		public override string Name { get { return "Uncanny Dodge"; } }
		public override int Level { get { return 5; } }

		public override string Description
		{
			get { return "Once per round, you halve the damage of one attack you can see coming."; }
		}

		public override int ReduceIncomingDamage(
			Mobile defender, IDnDCharacter character, int classLevel, int damage, bool ranged)
		{
			// Not worth a reaction for a scratch. Half of 3 is 1, and the round's reaction is worth
			// more than that against whatever lands next.
			if (damage < 4 || !DnDTurn.TrySpendReaction(defender))
			{
				return damage;
			}

			defender.SendMessage(0x35, "You twist aside, halving the blow.");

			return damage / 2;
		}
	}

	/// <summary>
	/// Evasion: a successful Dexterity save takes no damage at all rather than half.
	/// <para>
	/// Unlike the others this costs no reaction - the SRD does not charge one, because it applies
	/// to an effect you were already saving against. It is limited by how often something forces a
	/// Dexterity save on you instead.
	/// </para>
	/// </summary>
	public sealed class EvasionFeature : ClassFeature
	{
		public override string Name { get { return "Evasion"; } }
		public override int Level { get { return 7; } }

		public override string Description
		{
			get { return "When you succeed on a Dexterity save for half damage, you take none instead."; }
		}

		/// <summary>
		/// Called from the spell resolver rather than from ReduceIncomingDamage, because it needs
		/// to know the save was made and that is not something a damage number carries.
		/// </summary>
		public static int OnDexteritySave(Mobile defender, IDnDCharacter character, int damage, bool saved)
		{
			if (!saved || !ClassFeatures.Has(character, "Evasion"))
			{
				return damage;
			}

			defender.SendMessage(0x35, "You avoid the effect entirely.");

			return 0;
		}
	}

	/// <summary>Deflect Missiles: a Monk catches what is thrown at them.</summary>
	public sealed class DeflectMissilesFeature : ClassFeature
	{
		public override string Name { get { return "Deflect Missiles"; } }
		public override int Level { get { return 3; } }

		public override string Description
		{
			get { return "Once per round, you reduce the damage of a ranged attack by 1d10 plus your level."; }
		}

		public override int ReduceIncomingDamage(
			Mobile defender, IDnDCharacter character, int classLevel, int damage, bool ranged)
		{
			if (!ranged || !DnDTurn.TrySpendReaction(defender))
			{
				return damage;
			}

			int reduced = Math.Max(0, damage - (Utility.RandomMinMax(1, 10) + classLevel));

			if (reduced == 0)
			{
				defender.SendMessage(0x35, "You catch the missile out of the air.");
			}
			else
			{
				defender.SendMessage(0x35, "You knock the missile aside.");
			}

			return reduced;
		}
	}

	/// <summary>
	/// Cunning Action: the Rogue's bonus action, spent on getting away.
	/// <para>
	/// Dash and Disengage have no meaning without turn-based movement, so what this does here is
	/// the third option - Hide - expressed as the thing hiding is for: breaking off. Spending the
	/// bonus action clears your combatant and grants advantage on your next attack, which is what
	/// a Rogue slipping out of sight and coming back actually gets.
	/// </para>
	/// </summary>
	public sealed class CunningActionFeature : ClassFeature
	{
		public override string Name { get { return "Cunning Action"; } }
		public override int Level { get { return 2; } }

		public override string Description
		{
			get { return "Break off as a bonus action, gaining advantage on your next attack."; }
		}

		public override int GetUses(int classLevel) { return 1; }

		public override bool RecoversOnShortRest { get { return true; } }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			if (!DnDTurn.TrySpendBonusAction(user))
			{
				user.SendMessage("You have already used your bonus action this round.");
				return false;
			}

			user.Combatant = null;
			user.Warmode = false;

			DnDRollModifiers.AddAdvantage(user, RollKind.Attack, TimeSpan.FromSeconds(12.0), Name);

			user.SendMessage(0x35, "You slip out of the fight, ready to strike from cover.");

			return true;
		}
	}

	/// <summary>
	/// Riposte: a counterattack when a blow misses you.
	/// <para>
	/// The Battle Master's manoeuvre list is a resource pool of superiority dice in the SRD; this
	/// is the one manoeuvre whose trigger the engine can actually see, so it is the one built. The
	/// rest are noted in DND_TODO.md rather than approximated.
	/// </para>
	/// </summary>
	public sealed class RiposteFeature : ClassFeature
	{
		public override string Name { get { return "Riposte"; } }
		public override int Level { get { return 3; } }

		public override string Description
		{
			get { return "When an attack misses you, you may strike back as a reaction."; }
		}

		/// <summary>Called from the combat resolver on a miss. Returns true if the riposte lands.</summary>
		public static bool OnMissed(Mobile defender, Mobile attacker)
		{
			var character = defender as IDnDCharacter;

			if (!ClassFeatures.Has(character, "Riposte") || !DnDTurn.TrySpendReaction(defender))
			{
				return false;
			}

			defender.SendMessage(0x35, "You turn their miss into an opening.");

			return true;
		}
	}
}
