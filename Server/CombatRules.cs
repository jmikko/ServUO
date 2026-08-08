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
		/// characters get ability modifier + proficiency bonus, using Dex for ranged weapons and
		/// Str for everything else (SRD finesse is not modelled yet).
		/// </summary>
		public static int GetAttackBonus(Mobile attacker, bool ranged)
		{
			IDnDCreature creature = attacker as IDnDCreature;

			if (creature != null)
			{
				return creature.AttackBonus;
			}

			IDnDCharacter character = attacker as IDnDCharacter;

			if (character != null && character.CharacterClass != null)
			{
				int abilityMod = ranged ? character.AbilityScores.DexMod : character.AbilityScores.StrMod;

				return abilityMod + character.CharacterClass.GetProficiencyBonus(character.CharacterLevel);
			}

			return 0;
		}

		/// <summary>
		/// The damage bonus added to a weapon's dice. Monsters bake theirs into the dice expression,
		/// so they get none here.
		/// </summary>
		public static int GetDamageBonus(Mobile attacker, bool ranged)
		{
			if (attacker is IDnDCreature)
			{
				return 0;
			}

			IDnDCharacter character = attacker as IDnDCharacter;

			if (character != null)
			{
				return ranged ? character.AbilityScores.DexMod : character.AbilityScores.StrMod;
			}

			return 0;
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
