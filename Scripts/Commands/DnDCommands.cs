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
			CommandSystem.Register("spells", AccessLevel.Player, Spells_OnCommand);
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
			CharacterClass charClass = pm.CharacterClass;

			pm.SendMessage(0x35, "--- {0} ---", pm.Name);
			pm.SendMessage("Level {0} {1}{2}", pm.CharacterLevel, charClass.Name, SpeciesSuffix(pm));

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
				Signed(charClass.GetProficiencyBonus(pm.CharacterLevel)));

			int toNext = Advancement.GetExperienceToNextLevel(pm.Experience);

			pm.SendMessage(
				"Experience {0}{1}",
				pm.Experience,
				toNext > 0 ? String.Format(" ({0} to level {1})", toNext, pm.CharacterLevel + 1) : " (maximum level)");

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

			if (!pm.CharacterClass.CanCastSpells)
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

			if (!pm.DnDInitialized || pm.CharacterClass == null)
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
	}
}
