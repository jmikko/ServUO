#region References
using System;
using System.Collections.Generic;
#endregion

namespace Server
{
	public abstract class CharacterClass
	{
		private static readonly List<CharacterClass> m_AllClasses = new List<CharacterClass>();

		public static List<CharacterClass> AllClasses { get { return m_AllClasses; } }

		public static void Register(CharacterClass charClass)
		{
			if (!m_AllClasses.Contains(charClass))
			{
				m_AllClasses.Add(charClass);
			}
		}

		public static CharacterClass Parse(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				return null;
			}

			for (int i = 0; i < m_AllClasses.Count; ++i)
			{
				if (Insensitive.Equals(m_AllClasses[i].Name, value))
				{
					return m_AllClasses[i];
				}
			}

			return null;
		}

		public abstract string Name { get; }
		
		public virtual Type ParentClass { get { return null; } }

		/// <summary>
		/// The class' hit die, e.g. 10 for a d10.
		/// </summary>
		public abstract int HitDie { get; }

		public abstract AbilityScoreType[] SavingThrowProficiencies { get; }

		public abstract WeaponCategory WeaponProficiencies { get; }

		public abstract ArmorCategory ArmorProficiencies { get; }

		public abstract bool CanCastSpells { get; }

		/// <summary>
		/// The skills this class may take proficiency in at creation. A character picks
		/// <see cref="SkillChoiceCount"/> of them - which is what makes two Fighters different
		/// before either has gained a level.
		/// <para>
		/// A subclass inherits its parent's list unless it says otherwise, which is correct: a
		/// Champion is a Fighter and trains as one.
		/// </para>
		/// </summary>
		public virtual DnDSkill[] SkillChoices { get { return new DnDSkill[0]; } }

		public virtual int SkillChoiceCount { get { return 2; } }

		/// <summary>
		/// How fast this class gains spell slots. Non-casters leave this at None, which is what
		/// makes <see cref="Spellcasting.GetMaxSlots"/> hand them nothing at any level.
		/// </summary>
		public virtual SpellProgression SpellProgression { get { return SpellProgression.None; } }

		/// <summary>
		/// The ability that powers this class' magic - Int for Wizards, Wis for the divine and
		/// primal classes, Cha for the ones that cast on force of personality or a pact. Only
		/// meaningful when <see cref="SpellProgression"/> is not None.
		/// </summary>
		public virtual AbilityScoreType SpellcastingAbility { get { return AbilityScoreType.Int; } }

		public override string ToString()
		{
			return Name;
		}

		public virtual int GetProficiencyBonus(int level)
		{
			return 2 + ((Math.Max(1, level) - 1) / 4);
		}

		public virtual bool IsProficientSave(AbilityScoreType type)
		{
			AbilityScoreType[] profs = SavingThrowProficiencies;

			for (int i = 0; i < profs.Length; ++i)
			{
				if (profs[i] == type)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Overridable. Determines whether this class is proficient with the given weapon/armor item.
		/// Base implementation checks an item's DnD category (via IDnDEquipment) against this class'
		/// weapon/armor proficiency flags. Items with no DnD category assigned are treated as proficient
		/// (legacy UO items not yet classified do not get blocked).
		/// </summary>
		public virtual bool IsProficientWith(Item item)
		{
			IDnDEquipment dndItem = item as IDnDEquipment;

			if (dndItem == null)
			{
				return true;
			}

			if (dndItem.WeaponCategory != WeaponCategory.None)
			{
				return (WeaponProficiencies & dndItem.WeaponCategory) != 0;
			}

			if (dndItem.ArmorCategory != ArmorCategory.None)
			{
				return (ArmorProficiencies & dndItem.ArmorCategory) != 0;
			}

			return true;
		}

		/// <summary>
		/// Returns the number of Ability Score Improvement (ASI) points this class grants at the given level.
		/// Each ASI typically provides 2 points to spend. By default, classes get an ASI at levels 4, 8, 12, 16, 19.
		/// </summary>
		public virtual int GetAbilityScoreImprovements(int level)
		{
			if (level == 4 || level == 8 || level == 12 || level == 16 || level == 19)
			{
				return 2;
			}
			return 0;
		}

		/// <summary>
		/// Returns the maximum number of spells a character of this class knows at the given level.
		/// Non-casters or classes that prepare from their entire list (Cleric, Druid, Paladin) return 0 or int.MaxValue.
		/// Default is 0. Overridden by spells-known classes.
		/// </summary>
		public virtual int GetSpellsKnown(int level)
		{
			return 0;
		}
	}
}
