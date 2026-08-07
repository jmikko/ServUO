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
	}
}
