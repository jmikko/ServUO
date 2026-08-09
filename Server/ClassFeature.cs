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

		/// <summary>
		/// Extra dice rolled on a critical hit, beyond the usual doubling - Brutal Critical.
		/// </summary>
		public virtual int ExtraCriticalDice(int classLevel) { return 0; }

		/// <summary>A flat addition to attack rolls, such as the Archery fighting style's +2.</summary>
		public virtual int AttackBonus { get { return 0; } }

		/// <summary>A flat addition to armour class, such as the Defense fighting style's +1.</summary>
		public virtual int ArmorClassBonus { get { return 0; } }

		/// <summary>
		/// An unarmoured armour class this feature grants, or 0. A Barbarian's is 10 + Dex + Con and
		/// a Monk's is 10 + Dex + Wis; both apply only while wearing no armour, which is why this is
		/// a whole value rather than a bonus.
		/// </summary>
		public virtual int GetUnarmoredArmorClass(IDnDCharacter character, int classLevel) { return 0; }

		/// <summary>A flat addition to every saving throw - the Paladin's Aura of Protection.</summary>
		public virtual int GetSaveBonus(IDnDCharacter character, int classLevel) { return 0; }

		/// <summary>Whether this feature grants advantage on saves of a given ability.</summary>
		public virtual bool GrantsSaveAdvantage(AbilityScoreType ability) { return false; }

		/// <summary>Halves damage of these types while the feature is active.</summary>
		public virtual bool ResistsPhysicalDamage(IDnDCharacter character) { return false; }

		/// <summary>
		/// A chance to reduce incoming damage, returning what gets through.
		/// <para>
		/// This is where the reaction features live - Uncanny Dodge, Deflect Missiles, Evasion.
		/// They are separate from <see cref="ResistsPhysicalDamage"/> because that one is a standing
		/// state (Rage halves everything, always) while these are spent: the feature must decide
		/// whether this particular hit is worth its one reaction this round, and say so by calling
		/// DnDTurn.TrySpendReaction itself. A hook that only reported a fraction could not do that.
		/// </para>
		/// </summary>
		public virtual int ReduceIncomingDamage(
			Mobile defender, IDnDCharacter character, int classLevel, int damage, bool ranged)
		{
			return damage;
		}

		/// <summary>
		/// How many times this can be used between rests, or 0 if it is passive. An activated
		/// feature is invoked by name and spends one use.
		/// </summary>
		public virtual int GetUses(int classLevel) { return 0; }

		/// <summary>Whether a short rest restores its uses, or only a long one.</summary>
		public virtual bool RecoversOnShortRest { get { return true; } }

		/// <summary>
		/// Runs when an activated feature is used. Returns false if it could not take effect, which
		/// leaves the use unspent.
		/// </summary>
		public virtual bool Activate(Mobile user, IDnDCharacter character, int classLevel) { return false; }
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

		public static int GetAttackBonus(IDnDCharacter character)
		{
			int total = 0;

			foreach (var entry in GetActive(character))
			{
				total += entry.Key.AttackBonus;
			}

			return total;
		}

		public static int GetArmorClassBonus(IDnDCharacter character)
		{
			int total = 0;

			foreach (var entry in GetActive(character))
			{
				total += entry.Key.ArmorClassBonus;
			}

			return total;
		}

		/// <summary>The best unarmoured armour class any feature offers, or 0 for none.</summary>
		public static int GetUnarmoredArmorClass(IDnDCharacter character)
		{
			int best = 0;

			foreach (var entry in GetActive(character))
			{
				int value = entry.Key.GetUnarmoredArmorClass(character, entry.Value);

				if (value > best)
				{
					best = value;
				}
			}

			return best;
		}

		public static int GetSaveBonus(IDnDCharacter character)
		{
			int total = 0;

			foreach (var entry in GetActive(character))
			{
				total += entry.Key.GetSaveBonus(character, entry.Value);
			}

			return total;
		}

		public static bool HasSaveAdvantage(IDnDCharacter character, AbilityScoreType ability)
		{
			foreach (var entry in GetActive(character))
			{
				if (entry.Key.GrantsSaveAdvantage(ability))
				{
					return true;
				}
			}

			return false;
		}

		public static bool ResistsPhysicalDamage(IDnDCharacter character)
		{
			foreach (var entry in GetActive(character))
			{
				if (entry.Key.ResistsPhysicalDamage(character))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Runs the incoming damage past every feature that might reduce it, in turn.
		/// <para>
		/// Chained rather than taking the best: a Monk 7 who deflects a missile and then evades the
		/// same damage has spent a reaction to earn both, and each feature checks its own reaction
		/// budget. In practice the reaction is gone after the first, which is exactly the limit the
		/// SRD puts on stacking them.
		/// </para>
		/// </summary>
		public static int ReduceIncomingDamage(
			Mobile defender, IDnDCharacter character, int damage, bool ranged)
		{
			foreach (var entry in GetActive(character))
			{
				damage = entry.Key.ReduceIncomingDamage(defender, character, entry.Value, damage, ranged);
			}

			return Math.Max(0, damage);
		}

		/// <summary>Extra damage dice on a critical, beyond the usual doubling.</summary>
		public static int GetExtraCriticalDice(IDnDCharacter character)
		{
			int best = 0;

			foreach (var entry in GetActive(character))
			{
				int dice = entry.Key.ExtraCriticalDice(entry.Value);

				if (dice > best)
				{
					best = dice;
				}
			}

			return best;
		}

		/// <summary>Finds an activated feature by name, with the class level that granted it.</summary>
		public static ClassFeature Find(IDnDCharacter character, string featureName, out int classLevel)
		{
			classLevel = 0;

			foreach (var entry in GetActive(character))
			{
				if (Insensitive.Equals(entry.Key.Name, featureName))
				{
					classLevel = entry.Value;
					return entry.Key;
				}
			}

			return null;
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
