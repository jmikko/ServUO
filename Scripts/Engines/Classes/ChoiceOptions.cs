using System;

namespace Server.Engines.Classes
{
	/// <summary>
	/// The catalogue of level-up choices: fighting styles, expertise, invocations, pact boons and
	/// metamagic.
	/// <para>
	/// The fighting styles here replace the ones that sat unattached in PassiveFeatures.cs. They
	/// were written and deliberately not granted, because granting one automatically raised every
	/// martial character's armour class by 1 - the bug that made "a choice nobody made" the rule
	/// this file exists to enforce.
	/// </para>
	/// </summary>
	public static class ChoiceOptions
	{
		// Not public: ScriptCompiler invokes every public static Configure it finds, and ClassSystem
		// already calls this one. Public would register the whole catalogue twice.
		internal static void Configure()
		{
			RegisterFightingStyles();
			RegisterExpertise();
			RegisterInvocations();
			RegisterPactBoons();
			RegisterMetamagic();
		}

		private static void RegisterFightingStyles()
		{
			// Offered to all three martial classes, so no class is named - the entitlement table in
			// DnDChoices already decides who is owed one and when.
			Add(ChoiceKind.FightingStyle, "Defense", "While wearing armour, you gain +1 to Armor Class.");
			Add(ChoiceKind.FightingStyle, "Archery", "You gain +2 to attack rolls with ranged weapons.");
			Add(ChoiceKind.FightingStyle, "Duelling", "Wielding one weapon in one hand, you gain +2 damage.");
			Add(ChoiceKind.FightingStyle, "Great Weapon Fighting", "You may reroll 1s and 2s on two-handed damage.");
			Add(ChoiceKind.FightingStyle, "Protection", "You may impose disadvantage on an attack against a nearby ally.");
			Add(ChoiceKind.FightingStyle, "Two-Weapon Fighting", "You add your ability modifier to off-hand damage.");
		}

		/// <summary>
		/// Expertise is one option per skill, because which skill is the whole choice. Eighteen rows
		/// rather than one row and a follow-up prompt, so the pick is a single decision the client
		/// already knows how to present.
		/// </summary>
		private static void RegisterExpertise()
		{
			foreach (DnDSkill skill in Enum.GetValues(typeof(DnDSkill)))
			{
				var option = new DnDChoiceOption(
					ChoiceKind.Expertise,
					"Expertise: " + skill,
					String.Format("Your proficiency bonus is doubled for {0} checks.", skill),
					null,
					1);

				option.Skill = skill;

				DnDChoices.Register(option);
			}
		}

		private static void RegisterInvocations()
		{
			Add(ChoiceKind.Invocation, "Agonizing Blast", "You add your Charisma modifier to Eldritch Blast damage.", "Warlock", 2);
			Add(ChoiceKind.Invocation, "Armor of Shadows", "You may cast Mage Armor on yourself at will.", "Warlock", 2);
			Add(ChoiceKind.Invocation, "Devil's Sight", "You see normally in darkness, magical or not.", "Warlock", 2);
			Add(ChoiceKind.Invocation, "Eldritch Sight", "You may cast Detect Magic at will.", "Warlock", 2);
			Add(ChoiceKind.Invocation, "Fiendish Vigor", "You may cast False Life on yourself at will.", "Warlock", 2);
			Add(ChoiceKind.Invocation, "Mask of Many Faces", "You may cast Disguise Self at will.", "Warlock", 2);
			Add(ChoiceKind.Invocation, "Repelling Blast", "Eldritch Blast pushes what it hits away.", "Warlock", 2);
			Add(ChoiceKind.Invocation, "Thirsting Blade", "You attack twice when you take the Attack action.", "Warlock", 5);
			Add(ChoiceKind.Invocation, "Ascendant Step", "You may cast Levitate on yourself at will.", "Warlock", 9);
			Add(ChoiceKind.Invocation, "Otherworldly Leap", "You may cast Jump on yourself at will.", "Warlock", 9);
			Add(ChoiceKind.Invocation, "Bewitching Whispers", "You may cast Compulsion once per long rest.", "Warlock", 7);
			Add(ChoiceKind.Invocation, "Dreadful Word", "You may cast Confusion once per long rest.", "Warlock", 7);
			Add(ChoiceKind.Invocation, "Sculptor of Flesh", "You may cast Polymorph once per long rest.", "Warlock", 7);
			Add(ChoiceKind.Invocation, "Visions of Distant Realms", "You may cast Arcane Eye at will.", "Warlock", 15);
			Add(ChoiceKind.Invocation, "Witch Sight", "You see the true form of any shapechanger.", "Warlock", 15);
		}

		private static void RegisterPactBoons()
		{
			Add(ChoiceKind.PactBoon, "Pact of the Blade", "You conjure a weapon you are proficient with.", "Warlock", 3);
			Add(ChoiceKind.PactBoon, "Pact of the Chain", "You gain a familiar of an unusual kind.", "Warlock", 3);
			Add(ChoiceKind.PactBoon, "Pact of the Tome", "You gain a book of three cantrips from any list.", "Warlock", 3);
		}

		private static void RegisterMetamagic()
		{
			Add(ChoiceKind.Metamagic, "Empowered Spell", "Spend a sorcery point to add a d4 to a spell's effect.", "Sorcerer", 3);
			Add(ChoiceKind.Metamagic, "Quickened Spell", "Spend two sorcery points to cast without your action.", "Sorcerer", 3);
			Add(ChoiceKind.Metamagic, "Careful Spell", "Spend a sorcery point to spare allies from your spell.", "Sorcerer", 3);
			Add(ChoiceKind.Metamagic, "Distant Spell", "Spend a sorcery point to double a spell's range.", "Sorcerer", 3);
			Add(ChoiceKind.Metamagic, "Subtle Spell", "Spend a sorcery point to cast without word or gesture.", "Sorcerer", 3);
			Add(ChoiceKind.Metamagic, "Twinned Spell", "Spend sorcery points to target a second creature.", "Sorcerer", 3);
		}

		private static void Add(ChoiceKind kind, string name, string description, string className = null, int level = 1)
		{
			DnDChoices.Register(new DnDChoiceOption(kind, name, description, className, level));
		}
	}
}
