using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Engines.Classes.Features
{
	/// <summary>
	/// Tracks how many uses of each activated feature a character has left.
	/// <para>
	/// A side table keyed by mobile and feature name, for the same reason conditions and timed
	/// effects are: the alternative is a field per feature on the player, which does not scale past
	/// the first few and cannot be restored generically by a rest.
	/// </para>
	/// </summary>
	public static class FeatureUses
	{
		private static readonly Dictionary<Mobile, Dictionary<string, int>> m_Spent =
			new Dictionary<Mobile, Dictionary<string, int>>();

		public static int GetRemaining(Mobile m, IDnDCharacter character, ClassFeature feature, int classLevel)
		{
			int max = feature.GetUses(classLevel);

			if (max <= 0)
			{
				return 0;
			}

			Dictionary<string, int> spent;
			int used;

			if (m_Spent.TryGetValue(m, out spent) && spent.TryGetValue(feature.Name, out used))
			{
				return Math.Max(0, max - used);
			}

			return max;
		}

		public static bool Spend(Mobile m, ClassFeature feature)
		{
			Dictionary<string, int> spent;

			if (!m_Spent.TryGetValue(m, out spent))
			{
				m_Spent[m] = spent = new Dictionary<string, int>();
			}

			int used;
			spent.TryGetValue(feature.Name, out used);
			spent[feature.Name] = used + 1;

			return true;
		}

		/// <summary>
		/// Restores uses. A short rest returns only what recovers on one; a long rest returns
		/// everything, which is why the flag exists on the feature rather than here.
		/// </summary>
		public static void Restore(Mobile m, IDnDCharacter character, bool longRest)
		{
			Dictionary<string, int> spent;

			if (m == null || !m_Spent.TryGetValue(m, out spent))
			{
				return;
			}

			if (longRest)
			{
				m_Spent.Remove(m);
				return;
			}

			foreach (var entry in ClassFeatures.GetActive(character))
			{
				if (entry.Key.RecoversOnShortRest)
				{
					spent.Remove(entry.Key.Name);
				}
			}
		}

		public static void Clear(Mobile m)
		{
			m_Spent.Remove(m);
		}
	}

	/// <summary>
	/// SRD Second Wind: a Fighter regains 1d10 + level hit points, once per short rest. The
	/// Fighter's answer to attrition, and the reason they can keep going between fights.
	/// </summary>
	public sealed class SecondWindFeature : ClassFeature
	{
		public override string Name { get { return "Second Wind"; } }
		public override int Level { get { return 1; } }
		public override int GetUses(int classLevel) { return 1; }
		public override string Description { get { return "Regain 1d10 + your level in hit points."; } }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			if (user.Hits >= user.HitsMax)
			{
				user.SendMessage("You are already at full health.");
				return false;
			}

			int healed = Utility.Dice(1, 10, classLevel);

			user.Hits += healed;
			user.SendMessage(0x35, "You catch your breath and recover {0} hit points.", healed);

			return true;
		}
	}

	/// <summary>
	/// SRD Action Surge: an extra action, twice per short rest from 17th level.
	/// <para>
	/// There are no turns here to take an extra action in, so it resolves as an immediate extra
	/// attack against whatever the Fighter is fighting - which is what the action would have been
	/// spent on nine times out of ten.
	/// </para>
	/// </summary>
	public sealed class ActionSurgeFeature : ClassFeature
	{
		public override string Name { get { return "Action Surge"; } }
		public override int Level { get { return 2; } }
		public override string Description { get { return "Immediately attack again."; } }

		public override int GetUses(int classLevel) { return classLevel >= 17 ? 2 : 1; }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			var target = user.Combatant as Mobile;

			if (target == null || target.Deleted || !target.Alive)
			{
				user.SendMessage("You have nothing to attack.");
				return false;
			}

			user.SendMessage(0x35, "You surge forward with a burst of speed.");

			Items.DnDCombat.Resolve(user, target, user.Weapon as IDnDEquipment);

			return true;
		}
	}

	/// <summary>
	/// SRD Rage: bonus damage and resistance to physical damage while it lasts. The defining
	/// Barbarian feature - it is most of why they can stand in the front line.
	/// </summary>
	public sealed class RageFeature : ClassFeature
	{
		private static readonly Dictionary<Mobile, DateTime> m_Raging = new Dictionary<Mobile, DateTime>();

		public override string Name { get { return "Rage"; } }
		public override int Level { get { return 1; } }
		public override bool RecoversOnShortRest { get { return false; } }

		public override string Description
		{
			get { return "Bonus damage and resistance to weapon damage for a minute."; }
		}

		public override int GetUses(int classLevel)
		{
			if (classLevel >= 20) { return 99; }
			if (classLevel >= 17) { return 6; }
			if (classLevel >= 12) { return 5; }
			if (classLevel >= 6) { return 4; }
			if (classLevel >= 3) { return 3; }

			return 2;
		}

		public static bool IsRaging(Mobile m)
		{
			DateTime until;

			if (m == null || !m_Raging.TryGetValue(m, out until))
			{
				return false;
			}

			if (DateTime.UtcNow >= until)
			{
				m_Raging.Remove(m);
				m.SendMessage("Your rage subsides.");

				return false;
			}

			return true;
		}

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			if (IsRaging(user))
			{
				user.SendMessage("You are already raging.");
				return false;
			}

			m_Raging[user] = DateTime.UtcNow + TimeSpan.FromMinutes(1.0);

			user.SendMessage(0x35, "You fly into a rage.");

			return true;
		}

		public override string GetBonusDamage(IDnDCharacter character, int classLevel, RollMode mode)
		{
			if (!IsRaging(character as Mobile))
			{
				return null;
			}

			int bonus = classLevel >= 16 ? 4 : classLevel >= 9 ? 3 : 2;

			return "1d1+" + (bonus - 1);
		}

		public override bool ResistsPhysicalDamage(IDnDCharacter character)
		{
			return IsRaging(character as Mobile);
		}
	}

	/// <summary>
	/// SRD Lay on Hands: a Paladin heals from a pool of hit points worth five per level, refreshed
	/// on a long rest. Modelled as uses of a fixed heal, since there is no interface for choosing
	/// how much of a pool to spend.
	/// </summary>
	public sealed class LayOnHandsFeature : ClassFeature
	{
		public override string Name { get { return "Lay on Hands"; } }
		public override int Level { get { return 1; } }
		public override bool RecoversOnShortRest { get { return false; } }
		public override int GetUses(int classLevel) { return Math.Max(1, classLevel); }
		public override string Description { get { return "Heal 5 hit points by touch."; } }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			if (user.Hits >= user.HitsMax)
			{
				user.SendMessage("You are already at full health.");
				return false;
			}

			user.Hits += 5;
			user.SendMessage(0x35, "You lay hands upon yourself and heal 5 hit points.");

			return true;
		}
	}

	/// <summary>
	/// SRD Arcane Recovery: a Wizard regains spell slots on a short rest, once per day.
	/// </summary>
	public sealed class ArcaneRecoveryFeature : ClassFeature
	{
		public override string Name { get { return "Arcane Recovery"; } }
		public override int Level { get { return 1; } }
		public override bool RecoversOnShortRest { get { return false; } }
		public override int GetUses(int classLevel) { return 1; }
		public override string Description { get { return "Recover your expended spell slots."; } }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			var pm = user as DnDPlayerMobile;

			if (pm == null)
			{
				return false;
			}

			pm.RestoreAllSpellSlots();
			pm.SendMessage(0x35, "You recover your expended spell slots.");

			return true;
		}
	}

	/// <summary>
	/// SRD Bardic Inspiration: the Bard hands out a die that improves someone's next roll. Applied
	/// to the Bard here, since there is no party targeting interface yet.
	/// </summary>
	public sealed class BardicInspirationFeature : ClassFeature
	{
		public override string Name { get { return "Bardic Inspiration"; } }
		public override int Level { get { return 1; } }
		public override bool RecoversOnShortRest { get { return false; } }
		public override string Description { get { return "Gain a die to add to your next roll."; } }

		/// <summary>
		/// The SRD ties this to Charisma, which GetUses has no access to - it is handed only a class
		/// level, because uses are a property of the feature rather than of who holds it. Three is
		/// the count for a typical Bard's Charisma.
		/// </summary>
		public override int GetUses(int classLevel) { return 3; }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			int die = classLevel >= 15 ? 12 : classLevel >= 10 ? 10 : classLevel >= 5 ? 8 : 6;

			DnDRollModifiers.Add(
				user, Name, die, 0, 1, RollKind.Attack | RollKind.Save | RollKind.AbilityCheck,
				TimeSpan.FromMinutes(10.0), true);

			user.SendMessage(0x35, "You are inspired: add a d{0} to your next roll.", die);

			return true;
		}
	}

	/// <summary>
	/// SRD Channel Divinity: a Cleric's divine power, here turning undead - the use everyone
	/// actually reaches for.
	/// </summary>
	public sealed class ChannelDivinityFeature : ClassFeature
	{
		public override string Name { get { return "Channel Divinity"; } }
		public override int Level { get { return 2; } }
		public override int GetUses(int classLevel) { return classLevel >= 18 ? 3 : classLevel >= 6 ? 2 : 1; }
		public override string Description { get { return "Frighten nearby undead."; } }

		public override bool Activate(Mobile user, IDnDCharacter character, int classLevel)
		{
			int affected = 0;

			foreach (Mobile m in user.GetMobilesInRange(6))
			{
				if (m == user || m.Deleted || !m.Alive)
				{
					continue;
				}

				if (!CombatRules.CheckSave(m, AbilityScoreType.Wis, Spellcasting.GetSaveDC(character)))
				{
					DnDConditions.Add(m, DnDCondition.Frightened, TimeSpan.FromSeconds(60.0));
					++affected;
				}
			}

			user.SendMessage(0x35, "You channel divine power; {0} creature(s) recoil.", affected);

			return true;
		}
	}
}
