namespace Server.Engines.Classes
{
	public class BardClass : CharacterClass
	{
		public override string Name { get { return "Bard"; } }

		public override int HitDie { get { return 8; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Dex, AbilityScoreType.Cha }; }
		}

		public override WeaponCategory WeaponProficiencies
		{
			get { return WeaponCategory.SimpleMelee | WeaponCategory.SimpleRanged; }
		}

		public override ArmorCategory ArmorProficiencies { get { return ArmorCategory.Light; } }

		public override bool CanCastSpells { get { return true; } }

		public override SpellProgression SpellProgression { get { return SpellProgression.Full; } }

		public override AbilityScoreType SpellcastingAbility { get { return AbilityScoreType.Cha; } }

		private static readonly int[] m_SpellsKnown = { 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 15, 16, 16, 17, 17, 18, 18, 19, 22 };

		public override int GetSpellsKnown(int level)
		{
			if (level < 1) return 0;
			return m_SpellsKnown[System.Math.Min(level, m_SpellsKnown.Length) - 1];
		}

		public override int SkillChoiceCount { get { return 3; } }

		public override DnDSkill[] SkillChoices
		{
			get { return new[] { DnDSkill.Acrobatics, DnDSkill.Deception, DnDSkill.History, DnDSkill.Insight, DnDSkill.Investigation, DnDSkill.Performance, DnDSkill.Persuasion, DnDSkill.SleightOfHand, DnDSkill.Stealth }; }
		}

		public override ClassFeature[] Features
		{
			get { return new ClassFeature[] { new Features.BardicInspirationFeature() }; }
		}
	}
}
