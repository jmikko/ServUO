#region References
using System;
#endregion

namespace Server
{
	/// <summary>
	/// How a d20 is rolled. Advantage and disadvantage never stack and always cancel each other,
	/// however many sources apply - so this is a single state rather than a running total.
	/// </summary>
	public enum RollMode
	{
		Normal,
		Advantage,
		Disadvantage
	}

	public enum SpellSchool
	{
		Abjuration,
		Conjuration,
		Divination,
		Enchantment,
		Evocation,
		Illusion,
		Necromancy,
		Transmutation
	}

	/// <summary>
	/// How fast a class gains spell slots. Half-casters (Paladin, Ranger) advance at half a full
	/// caster's rate; Warlocks use Pact Magic, which is a different resource entirely - few slots,
	/// always at the highest level available, refreshed on a short rest.
	/// </summary>
	public enum SpellProgression
	{
		None,
		Full,
		Half,
		Pact
	}

	/// <summary>
	/// The SRD spellcasting rules: slot tables, save DCs, spell attack bonuses and cantrip scaling.
	/// <para>
	/// This is the whole of UO's magery replacement. Nothing here has a mana cost, a skill check or
	/// a spell circle - a spell either has a slot to spend or it does not.
	/// </para>
	/// </summary>
	public static class Spellcasting
	{
		public const int MaxSpellLevel = 9;

		/// <summary>
		/// Full-caster slots, indexed [classLevel - 1][spellLevel - 1]. Straight from the SRD table;
		/// half-casters read the same table at half their level.
		/// </summary>
		private static readonly int[][] FullCasterSlots =
		{
			new[] { 2, 0, 0, 0, 0, 0, 0, 0, 0 }, // 1
			new[] { 3, 0, 0, 0, 0, 0, 0, 0, 0 }, // 2
			new[] { 4, 2, 0, 0, 0, 0, 0, 0, 0 }, // 3
			new[] { 4, 3, 0, 0, 0, 0, 0, 0, 0 }, // 4
			new[] { 4, 3, 2, 0, 0, 0, 0, 0, 0 }, // 5
			new[] { 4, 3, 3, 0, 0, 0, 0, 0, 0 }, // 6
			new[] { 4, 3, 3, 1, 0, 0, 0, 0, 0 }, // 7
			new[] { 4, 3, 3, 2, 0, 0, 0, 0, 0 }, // 8
			new[] { 4, 3, 3, 3, 1, 0, 0, 0, 0 }, // 9
			new[] { 4, 3, 3, 3, 2, 0, 0, 0, 0 }, // 10
			new[] { 4, 3, 3, 3, 2, 1, 0, 0, 0 }, // 11
			new[] { 4, 3, 3, 3, 2, 1, 0, 0, 0 }, // 12
			new[] { 4, 3, 3, 3, 2, 1, 1, 0, 0 }, // 13
			new[] { 4, 3, 3, 3, 2, 1, 1, 0, 0 }, // 14
			new[] { 4, 3, 3, 3, 2, 1, 1, 1, 0 }, // 15
			new[] { 4, 3, 3, 3, 2, 1, 1, 1, 0 }, // 16
			new[] { 4, 3, 3, 3, 2, 1, 1, 1, 1 }, // 17
			new[] { 4, 3, 3, 3, 3, 1, 1, 1, 1 }, // 18
			new[] { 4, 3, 3, 3, 3, 2, 1, 1, 1 }, // 19
			new[] { 4, 3, 3, 3, 3, 2, 2, 1, 1 }  // 20
		};

		/// <summary>Pact Magic slot count by Warlock level (index = level - 1).</summary>
		private static readonly int[] PactSlotCount =
		{
			1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4
		};

		/// <summary>The single level every Pact Magic slot is cast at, by Warlock level.</summary>
		private static readonly int[] PactSlotLevel =
		{
			1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5
		};

		/// <summary>
		/// How many slots of <paramref name="spellLevel"/> a character of this progression and level
		/// has in total. Returns 0 for any combination the class cannot reach.
		/// </summary>
		public static int GetMaxSlots(SpellProgression progression, int classLevel, int spellLevel)
		{
			if (spellLevel < 1 || spellLevel > MaxSpellLevel || classLevel < 1)
			{
				return 0;
			}

			switch (progression)
			{
				case SpellProgression.Full:
					{
						return ReadFullCasterTable(classLevel, spellLevel);
					}
				case SpellProgression.Half:
					{
						// A half-caster has no magic at all at 1st level, then rounds UP: a 5th-level
						// Paladin casts as a 3rd-level full caster (4 first-level slots and 2 second),
						// not a 2nd. Rounding down here silently costs half-casters a spell level.
						return classLevel < 2 ? 0 : ReadFullCasterTable((classLevel + 1) / 2, spellLevel);
					}
				case SpellProgression.Pact:
					{
						int index = Math.Min(classLevel, PactSlotCount.Length) - 1;

						return PactSlotLevel[index] == spellLevel ? PactSlotCount[index] : 0;
					}
			}

			return 0;
		}

		private static int ReadFullCasterTable(int casterLevel, int spellLevel)
		{
			if (casterLevel < 1)
			{
				return 0;
			}

			return FullCasterSlots[Math.Min(casterLevel, FullCasterSlots.Length) - 1][spellLevel - 1];
		}

		/// <summary>The highest spell level this character can currently cast, or 0 for none.</summary>
		public static int GetHighestSlotLevel(SpellProgression progression, int classLevel)
		{
			for (int level = MaxSpellLevel; level >= 1; --level)
			{
				if (GetMaxSlots(progression, classLevel, level) > 0)
				{
					return level;
				}
			}

			return 0;
		}

		public static int GetCastingAbilityModifier(IDnDCharacter character)
		{
			if (character == null || character.CharacterClass == null)
			{
				return 0;
			}

			return GetModifier(character.AbilityScores, character.CharacterClass.SpellcastingAbility);
		}

		/// <summary>SRD: 8 + proficiency bonus + spellcasting ability modifier.</summary>
		public static int GetSaveDC(IDnDCharacter character)
		{
			if (character == null || character.CharacterClass == null)
			{
				return 8;
			}

			return 8 +
				   character.CharacterClass.GetProficiencyBonus(character.CharacterLevel) +
				   GetCastingAbilityModifier(character);
		}

		/// <summary>SRD: proficiency bonus + spellcasting ability modifier.</summary>
		public static int GetSpellAttackBonus(IDnDCharacter character)
		{
			if (character == null || character.CharacterClass == null)
			{
				return 0;
			}

			return character.CharacterClass.GetProficiencyBonus(character.CharacterLevel) +
				   GetCastingAbilityModifier(character);
		}

		/// <summary>
		/// Cantrips scale with character level rather than with slots: one damage die at 1st, and
		/// another at 5th, 11th and 17th.
		/// </summary>
		public static int GetCantripDice(int characterLevel)
		{
			if (characterLevel >= 17)
			{
				return 4;
			}

			if (characterLevel >= 11)
			{
				return 3;
			}

			if (characterLevel >= 5)
			{
				return 2;
			}

			return 1;
		}

		public static int GetModifier(AbilityScores scores, AbilityScoreType type)
		{
			switch (type)
			{
				case AbilityScoreType.Str:
					return scores.StrMod;
				case AbilityScoreType.Dex:
					return scores.DexMod;
				case AbilityScoreType.Con:
					return scores.ConMod;
				case AbilityScoreType.Int:
					return scores.IntMod;
				case AbilityScoreType.Wis:
					return scores.WisMod;
				case AbilityScoreType.Cha:
					return scores.ChaMod;
			}

			return 0;
		}
	}
}
