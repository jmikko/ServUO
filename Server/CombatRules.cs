#region References
using System;
using System.Globalization;
#endregion

namespace Server
{
	/// <summary>
	/// D&amp;D 5.5e combat resolution (d20 vs. AC, dice damage) and hard class-proficiency gating are
	/// unconditional now - there is no legacy UO combat path left to toggle between. Combatants
	/// without D&amp;D data (an un-set-up PlayerMobile, or a monster not implementing IDnDCreature)
	/// still resolve safely via GetDnDAttackBonus/GetDnDArmorClass's built-in fallbacks (+0 attack
	/// bonus, AC 10) in BaseWeapon.cs, rather than needing a separate "is this a D&amp;D combatant"
	/// gate.
	/// </summary>
	public static class CombatRules
	{
		/// <summary>
		/// Parses a simple "NdM" or "NdM+B" dice expression (e.g. "1d8", "2d4+1") and rolls it via
		/// Utility.Dice. Returns 0 for a malformed expression rather than throwing, since this is
		/// called from the hot combat path.
		/// </summary>
		public static int RollDice(string expression)
		{
			if (String.IsNullOrEmpty(expression))
			{
				return 0;
			}

			int dIndex = expression.IndexOf('d');

			if (dIndex <= 0)
			{
				return 0;
			}

			string countPart = expression.Substring(0, dIndex);

			string sidesPart = expression.Substring(dIndex + 1);
			int bonus = 0;

			int plusIndex = sidesPart.IndexOf('+');

			if (plusIndex >= 0)
			{
				string bonusPart = sidesPart.Substring(plusIndex + 1);
				sidesPart = sidesPart.Substring(0, plusIndex);

				int.TryParse(bonusPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out bonus);
			}

			int count, sides;

			if (!int.TryParse(countPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out count) ||
				!int.TryParse(sidesPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out sides))
			{
				return 0;
			}

			return Utility.Dice(count, sides, bonus);
		}

		/// <summary>
		/// The lowest and highest values a dice expression can produce. Used for display only -
		/// combat always goes through <see cref="RollDice"/>.
		/// </summary>
		public static void GetDiceRange(string expression, out int min, out int max)
		{
			min = 0;
			max = 0;

			if (String.IsNullOrEmpty(expression))
			{
				return;
			}

			int dIndex = expression.IndexOf('d');

			if (dIndex <= 0)
			{
				return;
			}

			string countPart = expression.Substring(0, dIndex);
			string sidesPart = expression.Substring(dIndex + 1);
			int bonus = 0;

			int plusIndex = sidesPart.IndexOf('+');

			if (plusIndex >= 0)
			{
				int.TryParse(
					sidesPart.Substring(plusIndex + 1), NumberStyles.Integer, CultureInfo.InvariantCulture, out bonus);

				sidesPart = sidesPart.Substring(0, plusIndex);
			}

			int count, sides;

			if (!int.TryParse(countPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out count) ||
				!int.TryParse(sidesPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out sides))
			{
				return;
			}

			min = count + bonus;
			max = (count * sides) + bonus;
		}

		/// <summary>
		/// The attacker's d20 attack-roll bonus. Monsters carry a flat bonus in their stat block;
		/// characters get ability modifier + proficiency bonus, using Dex for ranged weapons, Str
		/// for ordinary melee weapons, and the better of Str/Dex for finesse weapons.
		/// </summary>
		public static int GetAttackBonus(Mobile attacker, bool ranged, bool finesse = false, Item weapon = null)
		{
			int magicBonus = 0;
			if (weapon is IDnDMagicItem magicWeapon)
			{
				IDnDCharacter c = attacker as IDnDCharacter;
				if (!magicWeapon.RequiresAttunement || (c != null && c.IsAttunedTo(weapon)))
				{
					magicBonus += magicWeapon.AttackBonus;
				}
			}
			IDnDCreature creature = attacker as IDnDCreature;

			if (creature != null)
			{
				return creature.AttackBonus;
			}

			IDnDCharacter character = attacker as IDnDCharacter;

			if (character != null && character.PrimaryClass != null)
			{
				int abilityMod = GetWeaponAbilityModifier(character, ranged, finesse);

				// Fighting styles and their kin.
				return abilityMod + character.PrimaryClass.GetProficiencyBonus(character.TotalLevel) + magicBonus
					 + ClassFeatures.GetAttackBonus(character)
					 + Feat.GetAttackBonus(character);
			}

			return magicBonus;
		}

		/// <summary>
		/// The damage bonus added to a weapon's dice. Monsters bake theirs into the dice expression,
		/// so they get none here.
		/// </summary>
		public static int GetDamageBonus(Mobile attacker, bool ranged, bool finesse = false, Item weapon = null)
		{
			int magicBonus = 0;
			if (weapon is IDnDMagicItem magicWeapon)
			{
				IDnDCharacter c = attacker as IDnDCharacter;
				if (!magicWeapon.RequiresAttunement || (c != null && c.IsAttunedTo(weapon)))
				{
					magicBonus += magicWeapon.DamageBonus;
				}
			}

			if (attacker is IDnDCreature)
			{
				return magicBonus;
			}

			IDnDCharacter character = attacker as IDnDCharacter;

			if (character != null)
			{
				return GetWeaponAbilityModifier(character, ranged, finesse) + magicBonus;
			}

			return magicBonus;
		}

		private static int GetWeaponAbilityModifier(IDnDCharacter character, bool ranged, bool finesse)
		{
			if (ranged)
			{
				return character.EffectiveAbilityScores.DexMod;
			}

			return finesse
				? Math.Max(character.EffectiveAbilityScores.StrMod, character.EffectiveAbilityScores.DexMod)
				: character.EffectiveAbilityScores.StrMod;
		}

		/// <summary>
		/// Rolls a d20 with advantage (best of two), disadvantage (worst of two), or neither.
		/// Advantage and disadvantage never stack and always cancel out, however many sources apply.
		/// </summary>
		public static int RollD20(RollMode mode)
		{
			int first = Utility.RandomMinMax(1, 20);

			if (mode == RollMode.Normal)
			{
				return first;
			}

			int second = Utility.RandomMinMax(1, 20);

			return mode == RollMode.Advantage ? Math.Max(first, second) : Math.Min(first, second);
		}

		/// <summary>
		/// Rolls a saving throw: d20 + ability modifier, plus the proficiency bonus if the target's
		/// class is proficient in that save. Monsters have no per-ability save data in their stat
		/// blocks yet, so they roll flat d20 against the DC.
		/// </summary>
		public static bool CheckSave(Mobile target, AbilityScoreType ability, int dc)
		{
			return CheckSave(target, ability, dc, RollMode.Normal);
		}

		public static bool CheckSave(Mobile target, AbilityScoreType ability, int dc, RollMode mode)
		{
			// Paralysed, petrified, stunned and unconscious creatures do not get to roll at all
			// against anything physical.
			if (DnDConditions.AutoFailsSave(target, ability))
			{
				return false;
			}

			// Danger Sense and its relatives grant advantage on particular saves.
			IDnDCharacter saver = target as IDnDCharacter;

			if (mode == RollMode.Normal
				&& (ClassFeatures.HasSaveAdvantage(saver, ability)
				 || Feat.HasSaveAdvantage(saver, ability)
				 || DnDRollModifiers.HasAdvantage(target, RollKind.Save)))
			{
				mode = RollMode.Advantage;
			}

			// Bless and its relatives add a fresh die to the roll rather than a fixed number, so
			// they are rolled here rather than folded into the character's stats.
			int roll = RollD20(mode) + DnDRollModifiers.Roll(target, RollKind.Save);
			int bonus = 0;

			IDnDCharacter character = target as IDnDCharacter;

			if (character != null && character.DnDInitialized)
			{
				bonus = Spellcasting.GetModifier(character.EffectiveAbilityScores, ability);

				if (character.PrimaryClass != null && character.PrimaryClass.IsProficientSave(ability))
				{
					bonus += character.PrimaryClass.GetProficiencyBonus(character.TotalLevel);
				}

				// Aura of Protection and Diamond Soul.
				bonus += ClassFeatures.GetSaveBonus(character) + Feat.GetSaveBonusFor(character, ability);

				foreach (Item item in target.Items)
				{
					if (item is IDnDMagicItem magicItem)
					{
						if (!magicItem.RequiresAttunement || character.IsAttunedTo(item))
						{
							bonus += magicItem.SavingThrowBonus;
						}
					}
				}
			}

			return roll + bonus >= dc;
		}

		public static bool CheckAbility(Mobile target, AbilityScoreType ability, int dc, RollMode mode = RollMode.Normal)
		{
			if (mode == RollMode.Normal && DnDRollModifiers.HasAdvantage(target, RollKind.AbilityCheck))
			{
				mode = RollMode.Advantage;
			}

			// Bless does not apply to ability checks, but Guidance does.
			int roll = RollD20(mode) + DnDRollModifiers.Roll(target, RollKind.AbilityCheck);
			int bonus = 0;

			IDnDCharacter character = target as IDnDCharacter;

			if (character != null && character.DnDInitialized)
			{
				bonus = Spellcasting.GetModifier(character.EffectiveAbilityScores, ability);
			}

			return roll + bonus >= dc;
		}

		/// <summary>
		/// Rolls a skill check, which is an ability check that adds the proficiency bonus
		/// if the character is proficient.
		/// </summary>
		public static bool CheckSkill(Mobile target, DnDSkill skill, int dc, RollMode mode = RollMode.Normal)
		{
			AbilityScoreType ability = DnDSkills.GetPrimaryAbility(skill);

			if (mode == RollMode.Normal && DnDRollModifiers.HasAdvantage(target, RollKind.AbilityCheck))
			{
				mode = RollMode.Advantage;
			}

			int roll = RollD20(mode) + DnDRollModifiers.Roll(target, RollKind.AbilityCheck);
			int bonus = 0;

			IDnDCharacter character = target as IDnDCharacter;

			if (character != null && character.DnDInitialized)
			{
				bonus = Spellcasting.GetModifier(character.EffectiveAbilityScores, ability);

				if (character.IsProficient(skill) && character.PrimaryClass != null)
				{
					bonus += character.PrimaryClass.GetProficiencyBonus(character.TotalLevel);
				}
			}

			return roll + bonus >= dc;
		}

		/// <summary>Defender AC. Anything with no D&amp;D data at all sits at the SRD floor of 10.</summary>
		public static int GetArmorClass(IDamageable defender)
		{
			IDnDCreature creature = defender as IDnDCreature;

			if (creature != null)
			{
				return creature.ArmorClass;
			}

			IDnDCharacter character = defender as IDnDCharacter;

			if (character != null)
			{
				return character.ArmorClass;
			}

			return 10;
		}
	}
}
