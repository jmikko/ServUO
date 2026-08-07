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

		/// <summary>
		/// The class' hit die, e.g. 10 for a d10.
		/// </summary>
		public abstract int HitDie { get; }

		public abstract AbilityScoreType[] SavingThrowProficiencies { get; }

		public abstract WeaponCategory WeaponProficiencies { get; }

		public abstract ArmorCategory ArmorProficiencies { get; }

		public abstract bool CanCastSpells { get; }

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
	}
}
