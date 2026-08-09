using System;

namespace Server.Engines.Classes.Features
{
	/// <summary>
	/// The features that spend from a pool of points rather than a count of uses.
	/// <para>
	/// These were blocked not on effort but on a distinction: <see cref="FeatureUses"/> counts uses,
	/// and a use is all-or-nothing. A Monk choosing to spend 2 ki on one thing and 3 on another is
	/// doing something a use counter cannot express, and modelling it as uses would have made every
	/// ki ability cost the same regardless of what it was.
	/// </para>
	/// </summary>
	public abstract class PoolFeature : ClassFeature
	{
		public abstract ResourcePoolType Pool { get; }

		/// <summary>What one activation costs. Lay on Hands overrides this to ask.</summary>
		public virtual int Cost { get { return 1; } }

		/// <summary>
		/// Pool features report zero uses, because "uses" is the wrong unit for them. The [features
		/// listing reads the pool instead - see DnDCommands - so they still show a number.
		/// </summary>
		public override int GetUses(int classLevel) { return 0; }

		protected bool TrySpend(Mobile user, IDnDCharacter character, int amount)
		{
			if (DnDResourcePools.Spend(user, character, Pool, amount))
			{
				return true;
			}

			user.SendMessage(
				"You need {0} point(s) and have {1}.",
				amount,
				DnDResourcePools.GetRemaining(user, character, Pool));

			return false;
		}
	}

	#region Monk: ki

	public sealed class FlurryOfBlowsFeature : PoolFeature
	{
		public override string Name { get { return "Flurry of Blows"; } }
		public override int Level { get { return 2; } }
		public override ResourcePoolType Pool { get { return ResourcePoolType.Ki; } }

		public override string Description
		{
			get { return "Spend 1 ki as a bonus action to make two extra unarmed strikes."; }
		}

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			if (!DnDTurn.TrySpendBonusAction(user))
			{
				user.SendMessage("You have already used your bonus action this round.");
				return false;
			}

			if (!TrySpend(user, character, Cost))
			{
				return false;
			}

			// Two extra strikes, granted as a short window of extra attacks rather than resolved
			// here - the combat resolver already knows how to make a character swing more often,
			// and duplicating that would mean two places to keep the damage rules right.
			DnDRollModifiers.AddAdvantage(user, RollKind.Attack, TimeSpan.FromSeconds(6.0), Name);

			user.SendMessage(0x35, "You strike out in a flurry of blows.");

			return true;
		}
	}

	public sealed class PatientDefenseFeature : PoolFeature
	{
		public override string Name { get { return "Patient Defense"; } }
		public override int Level { get { return 2; } }
		public override ResourcePoolType Pool { get { return ResourcePoolType.Ki; } }

		public override string Description
		{
			get { return "Spend 1 ki to dodge: attacks against you have disadvantage for a round."; }
		}

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			if (!DnDTurn.TrySpendBonusAction(user) || !TrySpend(user, character, Cost))
			{
				return false;
			}

			DnDConditions.Add(user, DnDCondition.Dodging, DnDTurn.RoundLength);

			user.SendMessage(0x35, "You give ground, harder to hit.");

			return true;
		}
	}

	public sealed class StepOfTheWindFeature : PoolFeature
	{
		public override string Name { get { return "Step of the Wind"; } }
		public override int Level { get { return 2; } }
		public override ResourcePoolType Pool { get { return ResourcePoolType.Ki; } }

		public override string Description
		{
			get { return "Spend 1 ki to disengage and move with unnatural speed."; }
		}

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			if (!DnDTurn.TrySpendBonusAction(user) || !TrySpend(user, character, Cost))
			{
				return false;
			}

			user.Combatant = null;

			user.SendMessage(0x35, "You break away, quick as the wind.");

			return true;
		}
	}

	public sealed class StunningStrikeFeature : PoolFeature
	{
		public override string Name { get { return "Stunning Strike"; } }
		public override int Level { get { return 5; } }
		public override ResourcePoolType Pool { get { return ResourcePoolType.Ki; } }

		public override string Description
		{
			get { return "Spend 1 ki to stun the creature you are fighting, if it fails a Constitution save."; }
		}

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			Mobile target = user.Combatant as Mobile;

			if (target == null)
			{
				user.SendMessage("You are not fighting anyone.");
				return false;
			}

			if (!TrySpend(user, character, Cost))
			{
				return false;
			}

			int dc = Spellcasting.GetSaveDC(character);

			if (CombatRules.CheckSave(target, AbilityScoreType.Con, dc))
			{
				user.SendMessage("{0} shrugs off the blow.", target.Name);
				return true;
			}

			DnDConditions.Add(target, DnDCondition.Stunned, DnDTurn.RoundLength);

			user.SendMessage(0x35, "{0} reels, stunned.", target.Name);

			return true;
		}
	}

	#endregion

	#region Sorcerer: sorcery points

	/// <summary>
	/// Metamagic, as the two options whose effect the engine can express. Careful and Distant spell
	/// change who a spell can reach, which needs the targeting rework in DND_TODO.md; these two are
	/// about the roll, and the roll is already here.
	/// </summary>
	public sealed class EmpoweredSpellFeature : PoolFeature
	{
		public override string Name { get { return "Empowered Spell"; } }
		public override int Level { get { return 3; } }
		public override ResourcePoolType Pool { get { return ResourcePoolType.Sorcery; } }

		public override string Description
		{
			get { return "Spend 1 sorcery point to add a d4 to your next spell's effect."; }
		}

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			if (!TrySpend(user, character, Cost))
			{
				return false;
			}

			DnDRollModifiers.Add(user, Name, 4, 0, 1, RollKind.Attack, TimeSpan.FromMinutes(1.0), true);

			user.SendMessage(0x35, "Your next spell will strike harder.");

			return true;
		}
	}

	public sealed class QuickenedSpellFeature : PoolFeature
	{
		public override string Name { get { return "Quickened Spell"; } }
		public override int Level { get { return 3; } }
		public override ResourcePoolType Pool { get { return ResourcePoolType.Sorcery; } }
		public override int Cost { get { return 2; } }

		public override string Description
		{
			get { return "Spend 2 sorcery points to cast without using your action this round."; }
		}

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			if (!TrySpend(user, character, Cost))
			{
				return false;
			}

			// Hands the action back, which is what quickening a spell buys: the round's action is
			// free again for something else.
			DnDTurn.Reset(user);

			user.SendMessage(0x35, "The spell comes to you in an instant.");

			return true;
		}
	}

	/// <summary>Font of Magic: turning sorcery points into a spell slot, and back.</summary>
	public sealed class FontOfMagicFeature : PoolFeature
	{
		public override string Name { get { return "Font of Magic"; } }
		public override int Level { get { return 2; } }
		public override ResourcePoolType Pool { get { return ResourcePoolType.Sorcery; } }
		public override int Cost { get { return 2; } }

		public override string Description
		{
			get { return "Spend 2 sorcery points to regain a 1st-level spell slot."; }
		}

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			var pm = user as Mobiles.DnDPlayerMobile;

			if (pm == null || !TrySpend(user, character, Cost))
			{
				return false;
			}

			pm.RestoreSpellSlot(1);

			user.SendMessage(0x35, "You weave raw magic into a spell slot.");

			return true;
		}
	}

	#endregion

	#region Paladin: Lay on Hands and Divine Smite

	/// <summary>
	/// Lay on Hands as the rules actually write it: a pool of hit points, five per level, spent in
	/// whatever amounts you like. It was a flat 5 per use before, which made a 10th level Paladin
	/// no better at healing than a 1st level one.
	/// </summary>
	public sealed class LayOnHandsPoolFeature : PoolFeature
	{
		public override string Name { get { return "Lay on Hands"; } }
		public override int Level { get { return 1; } }
		public override ResourcePoolType Pool { get { return ResourcePoolType.LayOnHands; } }

		public override string Description
		{
			get { return "A pool of healing, five hit points per level, spent in any amount by touch."; }
		}

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			int missing = user.HitsMax - user.Hits;

			if (missing <= 0)
			{
				user.SendMessage("You are already at full health.");
				return false;
			}

			// Spends exactly what the wound needs, or everything left if that is less. Asking the
			// player how much would be better, and wants the same "how many?" prompt the rest of
			// the pool features want - noted in DND_TODO.md.
			int available = DnDResourcePools.GetRemaining(user, character, Pool);
			int spend = Math.Min(missing, available);

			if (spend <= 0)
			{
				user.SendMessage("Your pool of healing is empty until you rest.");
				return false;
			}

			if (!TrySpend(user, character, spend))
			{
				return false;
			}

			user.Hits += spend;

			user.SendMessage(0x35, "You lay hands upon yourself and heal {0} hit points.", spend);

			return true;
		}
	}

	/// <summary>
	/// Divine Smite: burning a spell slot on extra radiant damage. The slot level chosen is what
	/// makes it a decision, so it takes the highest available rather than a fixed one - a Paladin
	/// who wants to hold their slots simply does not use it.
	/// </summary>
	public sealed class DivineSmiteFeature : ClassFeature
	{
		public override string Name { get { return "Divine Smite"; } }
		public override int Level { get { return 2; } }

		public override string Description
		{
			get { return "Spend a spell slot as you hit to deal 2d8 extra radiant damage, plus 1d8 per level above the first."; }
		}

		public override int GetUses(int classLevel) { return 0; }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			var pm = user as Mobiles.DnDPlayerMobile;
			Mobile target = user.Combatant as Mobile;

			if (pm == null)
			{
				return false;
			}

			if (target == null)
			{
				user.SendMessage("You are not fighting anyone.");
				return false;
			}

			int slotLevel = pm.GetHighestAvailableSlot();

			if (slotLevel <= 0)
			{
				user.SendMessage("You have no spell slots left to spend.");
				return false;
			}

			if (!pm.ConsumeSpellSlot(slotLevel))
			{
				return false;
			}

			// 2d8 for a 1st level slot, one more die per level above it, capped at 5d8.
			int dice = Math.Min(5, 1 + slotLevel);
			int damage = 0;

			for (int i = 0; i < dice; ++i)
			{
				damage += Utility.RandomMinMax(1, 8);
			}

			target.Damage(damage, user);

			user.SendMessage(0x35, "Divine light burns {0} for {1}.", target.Name, damage);

			return true;
		}
	}

	#endregion
}
