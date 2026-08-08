using System;
using System.Text;
using Server.Mobiles;
using Server.Spells.DnD;

namespace Server.Commands
{
	/// <summary>
	/// Player-facing D&amp;D commands. These exist because the client has no D&amp;D UI beyond the
	/// creation screen yet - a character sheet gump and a rest button belong there eventually, but
	/// until then a player needs some way to read their own sheet and take a rest.
	/// </summary>
	public static class DnDCommands
	{
		public static void Initialize()
		{
			CommandSystem.Register("sheet", AccessLevel.Player, Sheet_OnCommand);
			CommandSystem.Register("rest", AccessLevel.Player, LongRest_OnCommand);
			CommandSystem.Register("shortrest", AccessLevel.Player, ShortRest_OnCommand);
			CommandSystem.Register("hitdie", AccessLevel.Player, HitDie_OnCommand);
			CommandSystem.Register("spells", AccessLevel.Player, Spells_OnCommand);
			CommandSystem.Register("Attune", AccessLevel.Player, Attune_OnCommand);
			CommandSystem.Register("Unattune", AccessLevel.Player, Unattune_OnCommand);

			CommandSystem.Register("features", AccessLevel.Player, Features_OnCommand);
			CommandSystem.Register("use", AccessLevel.Player, Use_OnCommand);

			CommandSystem.Register("XP", AccessLevel.GameMaster, XP_OnCommand);
			CommandSystem.Register("LevelUp", AccessLevel.GameMaster, LevelUp_OnCommand);
		}

		[Usage("XP <amount>")]
		[Description("Awards experience to yourself. Levels earned still have to be spent with [LevelUp.")]
		private static void XP_OnCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (!IsSetUp(pm))
			{
				return;
			}

			int amount = e.Length > 0 ? e.GetInt32(0) : 0;

			if (amount <= 0)
			{
				pm.SendMessage("Usage: [XP <amount>");
				return;
			}

			pm.AwardExperience(amount);

			pm.SendMessage(
				"Experience {0}, level {1}, {2} level(s) waiting to be spent.",
				pm.Experience,
				pm.TotalLevel,
				pm.PendingLevels);
		}

		/// <summary>
		/// [LevelUp - spends one pending level on a class.
		/// <para>
		/// The client's level-up window is the real way to do this; a level is a choice, and which
		/// class it goes into is what multiclassing is. This exists so advancement can be tested
		/// without the gump, and so a player whose client is out of date is not stuck with levels
		/// they can never spend.
		/// </para>
		/// </summary>
		[Usage("LevelUp [class]")]
		[Description("Spends one pending level. Defaults to your current class.")]
		private static void LevelUp_OnCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (!IsSetUp(pm))
			{
				return;
			}

			if (pm.PendingLevels <= 0)
			{
				int toNext = Advancement.GetExperienceToNextLevel(pm.Experience);

				pm.SendMessage(
					"You have no levels waiting. {0}",
					toNext > 0 ? String.Format("{0} more experience to level {1}.", toNext, pm.TotalLevel + 1) : "You are at the maximum level.");

				return;
			}

			CharacterClass into = e.Length > 0 ? CharacterClass.Parse(e.GetString(0)) : pm.PrimaryClass;

			if (into == null)
			{
				pm.SendMessage("No such class. Try one of: {0}", DescribeClasses());
				return;
			}

			if (!Engines.Classes.ClassSystem.ApplyLevelChoice(pm, into))
			{
				return;
			}

			pm.SendMessage(
				0x35,
				"Level {0} total. {1} level(s) still waiting.",
				pm.TotalLevel,
				pm.PendingLevels);
		}

		[Usage("features")]
		[Description("Lists the class features you have, and how many uses are left of each.")]
		private static void Features_OnCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (!IsSetUp(pm))
			{
				return;
			}

			var active = ClassFeatures.GetActive(pm);

			if (active.Count == 0)
			{
				pm.SendMessage("You have no class features yet.");
				return;
			}

			pm.SendMessage(0x35, "--- Features ---");

			foreach (var entry in active)
			{
				ClassFeature feature = entry.Key;
				int uses = feature.GetUses(entry.Value);

				if (uses > 0)
				{
					pm.SendMessage(
						"{0} ({1}/{2} uses) - {3}",
						feature.Name,
						Engines.Classes.Features.FeatureUses.GetRemaining(pm, pm, feature, entry.Value),
						uses,
						feature.Description);
				}
				else
				{
					pm.SendMessage("{0} - {1}", feature.Name, feature.Description);
				}
			}
		}

		[Usage("use <feature>")]
		[Description("Uses an activated class feature, such as Second Wind or Rage.")]
		private static void Use_OnCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (!IsSetUp(pm))
			{
				return;
			}

			if (e.Length == 0)
			{
				pm.SendMessage("Usage: [use <feature>. Try [features to see what you have.");
				return;
			}

			string wanted = String.Join(" ", e.Arguments);

			int classLevel;
			ClassFeature feature = ClassFeatures.Find(pm, wanted, out classLevel);

			if (feature == null)
			{
				pm.SendMessage("You have no feature called '{0}'.", wanted);
				return;
			}

			if (feature.GetUses(classLevel) <= 0)
			{
				pm.SendMessage("{0} is always active - there is nothing to use.", feature.Name);
				return;
			}

			if (Engines.Classes.Features.FeatureUses.GetRemaining(pm, pm, feature, classLevel) <= 0)
			{
				pm.SendMessage("You have no uses of {0} left. Rest to recover it.", feature.Name);
				return;
			}

			// The use is only spent if the feature actually did something - Second Wind at full
			// health should not cost a use.
			if (feature.Activate(pm, pm, classLevel))
			{
				Engines.Classes.Features.FeatureUses.Spend(pm, feature);
			}
		}

		private static string DescribeClasses()
		{
			var builder = new StringBuilder();

			foreach (CharacterClass c in CharacterClass.AllClasses)
			{
				if (builder.Length > 0)
				{
					builder.Append(", ");
				}

				builder.Append(c.Name);
			}

			return builder.ToString();
		}

		[Usage("sheet")]
		[Description("Shows your D&D character sheet.")]
		private static void Sheet_OnCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (!IsSetUp(pm))
			{
				return;
			}

			AbilityScores scores = pm.AbilityScores;
			CharacterClass charClass = pm.PrimaryClass;

			pm.SendMessage(0x35, "--- {0} ---", pm.Name);
			pm.SendMessage("Level {0} {1}{2}", pm.TotalLevel, charClass.Name, SpeciesSuffix(pm));

			pm.SendMessage(
				"Str {0} ({1})  Dex {2} ({3})  Con {4} ({5})",
				scores.Str, Signed(scores.StrMod),
				scores.Dex, Signed(scores.DexMod),
				scores.Con, Signed(scores.ConMod));

			pm.SendMessage(
				"Int {0} ({1})  Wis {2} ({3})  Cha {4} ({5})",
				scores.Int, Signed(scores.IntMod),
				scores.Wis, Signed(scores.WisMod),
				scores.Cha, Signed(scores.ChaMod));

			pm.SendMessage(
				"AC {0}   HP {1}/{2}   Proficiency {3}",
				pm.ArmorClass,
				pm.Hits,
				pm.HitsMax,
				Signed(charClass.GetProficiencyBonus(pm.TotalLevel)));

			int toNext = Advancement.GetExperienceToNextLevel(pm.Experience);

			pm.SendMessage(
				"Experience {0}{1}",
				pm.Experience,
				toNext > 0 ? String.Format(" ({0} to level {1})", toNext, pm.TotalLevel + 1) : " (maximum level)");

			if (charClass.CanCastSpells)
			{
				pm.SendMessage(
					"Spell save DC {0}   Spell attack {1}",
					Spellcasting.GetSaveDC(pm),
					Signed(Spellcasting.GetSpellAttackBonus(pm)));

				string slots = DescribeSlots(pm);

				pm.SendMessage("Spell slots: {0}", slots.Length > 0 ? slots : "none");
			}
		}

		[Usage("spells")]
		[Description("Lists the spells you can cast.")]
		private static void Spells_OnCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (!IsSetUp(pm))
			{
				return;
			}

			if (!pm.PrimaryClass.CanCastSpells)
			{
				pm.SendMessage("You are not a spellcaster.");
				return;
			}

			var available = SpellRegistry.GetAvailable(pm);

			if (available.Count == 0)
			{
				pm.SendMessage("You know no spells yet.");
				return;
			}

			pm.SendMessage(0x35, "--- Spells ---");

			foreach (DnDSpell spell in available)
			{
				pm.SendMessage(
					"{0} ({1}, {2})",
					spell.Name,
					spell.IsCantrip ? "cantrip" : "level " + spell.Level,
					spell.School);
			}
		}

		[Usage("rest")]
		[Description("Takes a long rest: hit points and spell slots return.")]
		private static void LongRest_OnCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (!IsSetUp(pm) || !CanRest(pm))
			{
				return;
			}

			pm.LongRest();
		}

		[Usage("shortrest")]
		[Description("Takes a short rest.")]
		private static void ShortRest_OnCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (!IsSetUp(pm) || !CanRest(pm))
			{
				return;
			}

			pm.ShortRest();
		}

		/// <summary>
		/// Spending hit dice is the other half of a short rest, and the half that matters to a class
		/// with no magic to recover. One die per call, because the rules let you look at the result
		/// before deciding whether to spend another.
		/// </summary>
		[Usage("hitdie")]
		[Description("Spends one hit die to heal. A long rest restores them.")]
		private static void HitDie_OnCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (!IsSetUp(pm) || !CanRest(pm))
			{
				return;
			}

			pm.SpendHitDie();
		}

		/// <summary>You cannot rest with something actively trying to kill you.</summary>
		private static bool CanRest(DnDPlayerMobile pm)
		{
			if (!pm.Alive)
			{
				pm.SendMessage("You cannot rest while dead.");
				return false;
			}

			if (pm.Combatant != null || pm.Aggressors.Count > 0)
			{
				pm.SendMessage("You cannot rest while in combat.");
				return false;
			}

			return true;
		}

		private static bool IsSetUp(DnDPlayerMobile pm)
		{
			if (pm == null)
			{
				return false;
			}

			if (!pm.DnDInitialized || pm.PrimaryClass == null)
			{
				pm.SendMessage("Your character has not been set up yet.");
				return false;
			}

			return true;
		}

		private static string SpeciesSuffix(DnDPlayerMobile pm)
		{
			return pm.Race == null ? "" : " (" + pm.Race.Name + ")";
		}

		private static string DescribeSlots(DnDPlayerMobile pm)
		{
			var builder = new StringBuilder();

			for (int level = 1; level <= Spellcasting.MaxSpellLevel; ++level)
			{
				int max = pm.GetMaxSpellSlots(level);

				if (max <= 0)
				{
					continue;
				}

				if (builder.Length > 0)
				{
					builder.Append("  ");
				}

				builder.AppendFormat("L{0} {1}/{2}", level, pm.GetAvailableSpellSlots(level), max);
			}

			return builder.ToString();
		}

		private static string Signed(int value)
		{
			return value >= 0 ? "+" + value : value.ToString();
		}

		[Usage("Attune")]
		[Description("Attunes to a magical item. You can attune up to 3 items.")]
		private static void Attune_OnCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (!IsSetUp(pm))
			{
				return;
			}

			if (pm.AttunedItems.Count >= 3)
			{
				pm.SendMessage("You are already attuned to 3 items. You must unattune from one first.");
				return;
			}

			pm.SendMessage("Target the item you wish to attune to.");
			pm.Target = new AttuneTarget(pm);
		}

		[Usage("Unattune")]
		[Description("Removes attunement from a magical item.")]
		private static void Unattune_OnCommand(CommandEventArgs e)
		{
			DnDPlayerMobile pm = e.Mobile as DnDPlayerMobile;

			if (!IsSetUp(pm))
			{
				return;
			}

			if (pm.AttunedItems.Count == 0)
			{
				pm.SendMessage("You are not attuned to any items.");
				return;
			}

			pm.SendMessage("Target the item you wish to unattune from.");
			pm.Target = new UnattuneTarget(pm);
		}
	}

	public class AttuneTarget : Server.Targeting.Target
	{
		private DnDPlayerMobile m_Mobile;

		public AttuneTarget(DnDPlayerMobile m) : base(-1, false, Server.Targeting.TargetFlags.None)
		{
			m_Mobile = m;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			if (targeted is Item item)
			{
				if (!item.IsChildOf(m_Mobile.Backpack) && item.Parent != m_Mobile)
				{
					from.SendMessage("The item must be in your pack or equipped to attune to it.");
					return;
				}

				if (targeted is IDnDMagicItem magicItem)
				{
					if (!magicItem.RequiresAttunement)
					{
						from.SendMessage("That item does not require attunement.");
						return;
					}

					if (m_Mobile.AttunedItems.Contains(item))
					{
						from.SendMessage("You are already attuned to that item.");
						return;
					}

					m_Mobile.AttunedItems.Add(item);
					from.SendMessage($"You attune to {item.Name ?? "the item"}.");
					m_Mobile.Delta(MobileDelta.Armor | MobileDelta.Hits | MobileDelta.Stat);
				}
				else
				{
					from.SendMessage("That item does not require attunement.");
				}
			}
			else
			{
				from.SendMessage("You can only attune to items.");
			}
		}
	}

	public class UnattuneTarget : Server.Targeting.Target
	{
		private DnDPlayerMobile m_Mobile;

		public UnattuneTarget(DnDPlayerMobile m) : base(-1, false, Server.Targeting.TargetFlags.None)
		{
			m_Mobile = m;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			if (targeted is Item item)
			{
				if (m_Mobile.AttunedItems.Contains(item))
				{
					m_Mobile.AttunedItems.Remove(item);
					from.SendMessage($"You unattune from {item.Name ?? "the item"}.");
					m_Mobile.Delta(MobileDelta.Armor | MobileDelta.Hits | MobileDelta.Stat);
				}
				else
				{
					from.SendMessage("You are not attuned to that item.");
				}
			}
		}
	}
}
