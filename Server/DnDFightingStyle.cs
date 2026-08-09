using System;

namespace Server
{
	/// <summary>
	/// What a chosen fighting style does to the rolls.
	/// <para>
	/// The styles were written once before as <c>ClassFeature</c> subclasses and deliberately left
	/// unattached, because there was no way to choose one and granting them automatically raised
	/// every martial character's armour class. Now that <see cref="DnDChoices"/> exists, the choice
	/// is real - and the effect belongs here rather than in a feature, because a feature is granted
	/// by a class level and a style is picked from a list.
	/// </para>
	/// <para>
	/// Each is conditional on the weapon, which is the whole reason <see cref="WeaponContext"/>
	/// exists: Archery is +2 to ranged attacks, not +2 to attacks.
	/// </para>
	/// </summary>
	public static class DnDFightingStyles
	{
		public static int GetAttackBonus(IDnDCharacter character, WeaponContext weapon)
		{
			if (character == null)
			{
				return 0;
			}

			int bonus = 0;

			if (weapon.Ranged && DnDChoices.HasChosen(character, "Archery"))
			{
				bonus += 2;
			}

			return bonus;
		}

		public static int GetDamageBonus(IDnDCharacter character, WeaponContext weapon)
		{
			if (character == null)
			{
				return 0;
			}

			int bonus = 0;

			// Duelling wants one weapon in one hand and nothing in the other - a condition that
			// could not be checked at all before the weapon context existed.
			if (DnDChoices.HasChosen(character, "Duelling")
				&& !weapon.Ranged && !weapon.TwoHanded && !weapon.Unarmed && weapon.OffHandFree)
			{
				bonus += 2;
			}

			return bonus;
		}

		/// <summary>
		/// Defense: +1 armour class while wearing armour. The armour condition is what makes it a
		/// trade rather than a free point, and is why this reads the worn armour rather than simply
		/// returning 1 - which is exactly the bug that got the automatic version reverted.
		/// </summary>
		public static int GetArmorClassBonus(IDnDCharacter character, bool wearingArmor)
		{
			if (character == null || !wearingArmor)
			{
				return 0;
			}

			return DnDChoices.HasChosen(character, "Defense") ? 1 : 0;
		}

		/// <summary>
		/// Great Weapon Fighting: rerolls 1s and 2s on the damage dice of a two-handed weapon.
		/// Applied to a rolled total rather than per die, because the resolver rolls a dice
		/// expression rather than handing back individual dice - close enough in expectation, and
		/// noted as an approximation rather than passed off as the rule.
		/// </summary>
		public static int RerollLowDamage(IDnDCharacter character, WeaponContext weapon, string dice, int rolled)
		{
			if (character == null || !weapon.TwoHanded
				|| !DnDChoices.HasChosen(character, "Great Weapon Fighting"))
			{
				return rolled;
			}

			int minimum, maximum;

			CombatRules.GetDiceRange(dice, out minimum, out maximum);

			if (maximum <= minimum)
			{
				return rolled;
			}

			// Only a roll in the bottom fifth of the range is worth rerolling, which is roughly
			// where "every die showed a 1 or a 2" lands.
			int threshold = minimum + Math.Max(1, (maximum - minimum) / 5);

			if (rolled > threshold)
			{
				return rolled;
			}

			int rerolled = CombatRules.RollDice(dice);

			return Math.Max(rolled, rerolled);
		}
	}
}
