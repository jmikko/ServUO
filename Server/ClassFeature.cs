#region References
using System;
using System.Collections.Generic;
#endregion

namespace Server
{
	/// <summary>
	/// Something a class grants at a given level.
	/// <para>
	/// Features are the whole of what makes classes play differently - without them a class is a
	/// hit die, a set of proficiencies and a spell list, and a Fighter and a Barbarian are the same
	/// character with different labels.
	/// </para>
	/// <para>
	/// Each hook defaults to "changes nothing", so a feature overrides only the number it actually
	/// affects. They are deliberately expressed as numbers combat already uses rather than as
	/// callbacks that mutate state: a feature that cannot be seen in an attack roll, a damage roll
	/// or a critical threshold is a feature nobody can tell they have.
	/// </para>
	/// </summary>
	public abstract class ClassFeature
	{
		public abstract string Name { get; }

		/// <summary>The class level at which this is gained.</summary>
		public abstract int Level { get; }

		public virtual string Description { get { return String.Empty; } }

		/// <summary>Additional attacks per attack action, beyond the first.</summary>
		public virtual int ExtraAttacks { get { return 0; } }

		/// <summary>
		/// The lowest natural d20 roll that counts as a critical hit. 20 normally; a Champion's
		/// Improved Critical lowers it.
		/// </summary>
		public virtual int CriticalThreshold { get { return 20; } }

		/// <summary>
		/// Extra damage dice this feature contributes to a hit, as a dice expression, or null.
		/// <paramref name="mode"/> is how the attack was rolled, which is what Sneak Attack keys off.
		/// </summary>
		public virtual string GetBonusDamage(IDnDCharacter character, int classLevel, RollMode mode)
		{
			return null;
		}
	}

	/// <summary>
	/// Gathers the features a character currently has across every class they hold.
	/// <para>
	/// Multiclassing makes this per-class rather than per-character: a Fighter 5 / Wizard 3 gets
	/// the Fighter's 5th-level features and the Wizard's 3rd-level ones, not either class' features
	/// at level 8.
	/// </para>
	/// </summary>
	public static class ClassFeatures
	{
		/// <summary>Every active feature, paired with the level of the class that granted it.</summary>
		public static List<KeyValuePair<ClassFeature, int>> GetActive(IDnDCharacter character)
		{
			var active = new List<KeyValuePair<ClassFeature, int>>();

			if (character == null || character.Classes == null)
			{
				return active;
			}

			foreach (var entry in character.Classes)
			{
				CharacterClass characterClass = entry.Key;
				int classLevel = entry.Value;

				if (characterClass == null)
				{
					continue;
				}

				foreach (ClassFeature feature in characterClass.Features)
				{
					if (feature.Level <= classLevel)
					{
						active.Add(new KeyValuePair<ClassFeature, int>(feature, classLevel));
					}
				}
			}

			return active;
		}

		/// <summary>
		/// Extra attacks per action. The largest wins rather than the sum - a Fighter 5 / Ranger 5
		/// attacks twice, not three times, because Extra Attack does not stack with itself.
		/// </summary>
		public static int GetExtraAttacks(IDnDCharacter character)
		{
			int best = 0;

			foreach (var entry in GetActive(character))
			{
				if (entry.Key.ExtraAttacks > best)
				{
					best = entry.Key.ExtraAttacks;
				}
			}

			return best;
		}

		/// <summary>The lowest natural roll that crits - the most generous feature wins.</summary>
		public static int GetCriticalThreshold(IDnDCharacter character)
		{
			int threshold = 20;

			foreach (var entry in GetActive(character))
			{
				if (entry.Key.CriticalThreshold < threshold)
				{
					threshold = entry.Key.CriticalThreshold;
				}
			}

			return Math.Max(2, threshold);
		}

		/// <summary>Total extra damage from every feature that applies to this hit.</summary>
		public static int RollBonusDamage(IDnDCharacter character, RollMode mode)
		{
			int total = 0;

			foreach (var entry in GetActive(character))
			{
				string dice = entry.Key.GetBonusDamage(character, entry.Value, mode);

				if (!String.IsNullOrEmpty(dice))
				{
					total += CombatRules.RollDice(dice);
				}
			}

			return total;
		}

		public static bool Has(IDnDCharacter character, string featureName)
		{
			foreach (var entry in GetActive(character))
			{
				if (Insensitive.Equals(entry.Key.Name, featureName))
				{
					return true;
				}
			}

			return false;
		}
	}
}
