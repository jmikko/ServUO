using System;
using System.Collections.Generic;

namespace Server
{
	/// <summary>What kind of pick a character is being asked to make.</summary>
	public enum ChoiceKind
	{
		/// <summary>Fighting Style, at Fighter 1 / Paladin 2 / Ranger 2.</summary>
		FightingStyle,

		/// <summary>Expertise: doubles proficiency on a skill. Rogue 1 and 6, Bard 3 and 10.</summary>
		Expertise,

		/// <summary>Eldritch Invocations, Warlock 2 and every other level after.</summary>
		Invocation,

		/// <summary>Pact Boon, Warlock 3.</summary>
		PactBoon,

		/// <summary>Metamagic options, Sorcerer 3 and 10.</summary>
		Metamagic
	}

	/// <summary>One option a character can pick, offered by a class at a level.</summary>
	public sealed class DnDChoiceOption
	{
		public ChoiceKind Kind;
		public string Name;
		public string Description;

		/// <summary>The class that offers it, and the level at which it becomes available.</summary>
		public string ClassName;

		public int Level;

		/// <summary>Skill this option attaches to, for Expertise. Ignored otherwise.</summary>
		public DnDSkill Skill;

		public DnDChoiceOption(ChoiceKind kind, string name, string description, string className, int level)
		{
			Kind = kind;
			Name = name;
			Description = description;
			ClassName = className;
			Level = level;
		}
	}

	/// <summary>
	/// Choices a character makes at level-up, beyond the class and the ability scores.
	/// <para>
	/// The level-up window already collects a class, ability improvements and spells known. Fighting
	/// styles, expertise, invocations, pact boons and metamagic all want the same treatment - a list
	/// to choose from and a record of what was chosen - and building five separate mechanisms for
	/// that would have been five places to get the "how many are you owed" arithmetic wrong.
	/// </para>
	/// <para>
	/// Granting these automatically was tried once, for fighting styles, and reverted: it silently
	/// raised every martial character's armour class, which is exactly the failure mode this whole
	/// codebase keeps producing. A choice nobody made is not a choice.
	/// </para>
	/// </summary>
	public static class DnDChoices
	{
		private static readonly List<DnDChoiceOption> m_Options = new List<DnDChoiceOption>();

		public static IReadOnlyList<DnDChoiceOption> AllOptions { get { return m_Options; } }

		public static void Register(DnDChoiceOption option)
		{
			if (option != null && Find(option.Name) == null)
			{
				m_Options.Add(option);
			}
		}

		public static DnDChoiceOption Find(string name)
		{
			if (String.IsNullOrEmpty(name))
			{
				return null;
			}

			foreach (DnDChoiceOption option in m_Options)
			{
				if (Insensitive.Equals(option.Name, name))
				{
					return option;
				}
			}

			return null;
		}

		/// <summary>
		/// How many picks of a kind a character is owed in total, from their class levels. Compared
		/// against what they have already taken to find what they still owe.
		/// </summary>
		public static int GetEntitlement(IDnDCharacter character, ChoiceKind kind)
		{
			if (character == null || character.Classes == null)
			{
				return 0;
			}

			int total = 0;

			foreach (var kv in character.Classes)
			{
				total += GetEntitlementFor(ClassLineage(kv.Key), kv.Value, kind);
			}

			return total;
		}

		private static string ClassLineage(CharacterClass c)
		{
			// A subclass counts as its parent: a Way of the Open Hand monk is a Monk, and their
			// entitlements should not vanish the moment they specialise.
			while (c != null && c.GetParent() != null)
			{
				c = c.GetParent();
			}

			return c != null ? c.Name : null;
		}

		private static int GetEntitlementFor(string className, int classLevel, ChoiceKind kind)
		{
			if (className == null)
			{
				return 0;
			}

			switch (kind)
			{
				case ChoiceKind.FightingStyle:
					{
						if (Insensitive.Equals(className, "Fighter")) return classLevel >= 1 ? 1 : 0;
						if (Insensitive.Equals(className, "Paladin")) return classLevel >= 2 ? 1 : 0;
						if (Insensitive.Equals(className, "Ranger")) return classLevel >= 2 ? 1 : 0;

						return 0;
					}
				case ChoiceKind.Expertise:
					{
						// Two skills at Rogue 1 and two more at 6; Bard gets its at 3 and 10.
						if (Insensitive.Equals(className, "Rogue"))
						{
							return (classLevel >= 1 ? 2 : 0) + (classLevel >= 6 ? 2 : 0);
						}

						if (Insensitive.Equals(className, "Bard"))
						{
							return (classLevel >= 3 ? 2 : 0) + (classLevel >= 10 ? 2 : 0);
						}

						return 0;
					}
				case ChoiceKind.Invocation:
					{
						if (!Insensitive.Equals(className, "Warlock") || classLevel < 2)
						{
							return 0;
						}

						// Two at 2nd, then one more at 5, 7, 9, 12, 15 and 18.
						int known = 2;

						foreach (int level in new[] { 5, 7, 9, 12, 15, 18 })
						{
							if (classLevel >= level) ++known;
						}

						return known;
					}
				case ChoiceKind.PactBoon:
					{
						return Insensitive.Equals(className, "Warlock") && classLevel >= 3 ? 1 : 0;
					}
				case ChoiceKind.Metamagic:
					{
						if (!Insensitive.Equals(className, "Sorcerer"))
						{
							return 0;
						}

						return (classLevel >= 3 ? 2 : 0) + (classLevel >= 10 ? 1 : 0) + (classLevel >= 17 ? 1 : 0);
					}
			}

			return 0;
		}

		/// <summary>
		/// The options this character could legally take now: the right kind, from a class they have
		/// levels in, at a level they have reached, and not already taken.
		/// </summary>
		public static List<DnDChoiceOption> GetAvailable(IDnDCharacter character, ChoiceKind kind)
		{
			var available = new List<DnDChoiceOption>();

			if (character == null)
			{
				return available;
			}

			foreach (DnDChoiceOption option in m_Options)
			{
				if (option.Kind != kind || HasChosen(character, option.Name))
				{
					continue;
				}

				// An option with no class named is open to anyone who qualifies for the kind.
				if (option.ClassName != null && GetClassLevel(character, option.ClassName) < option.Level)
				{
					continue;
				}

				available.Add(option);
			}

			return available;
		}

		private static int GetClassLevel(IDnDCharacter character, string className)
		{
			int total = 0;

			foreach (var kv in character.Classes)
			{
				if (Insensitive.Equals(ClassLineage(kv.Key), className))
				{
					total += kv.Value;
				}
			}

			return total;
		}

		public static bool HasChosen(IDnDCharacter character, string optionName)
		{
			if (character == null || character.Choices == null)
			{
				return false;
			}

			foreach (string chosen in character.Choices)
			{
				if (Insensitive.Equals(chosen, optionName))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>How many of a kind this character has already taken.</summary>
		public static int GetChosenCount(IDnDCharacter character, ChoiceKind kind)
		{
			if (character == null || character.Choices == null)
			{
				return 0;
			}

			int count = 0;

			foreach (string chosen in character.Choices)
			{
				DnDChoiceOption option = Find(chosen);

				if (option != null && option.Kind == kind)
				{
					++count;
				}
			}

			return count;
		}

		/// <summary>How many picks of a kind are still owed, which is what a UI should prompt for.</summary>
		public static int GetPending(IDnDCharacter character, ChoiceKind kind)
		{
			return Math.Max(0, GetEntitlement(character, kind) - GetChosenCount(character, kind));
		}
	}
}
