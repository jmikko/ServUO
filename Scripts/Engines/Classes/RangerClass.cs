namespace Server.Engines.Classes
{
	public class RangerClass : CharacterClass
	{
		public override string Name { get { return "Ranger"; } }

		public override int HitDie { get { return 10; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Str, AbilityScoreType.Dex }; }
		}

		public override WeaponCategory WeaponProficiencies { get { return WeaponCategory.All; } }

		public override ArmorCategory ArmorProficiencies
		{
			get { return ArmorCategory.Light | ArmorCategory.Medium | ArmorCategory.Shield; }
		}

		public override bool CanCastSpells { get { return true; } }

		public override SpellProgression SpellProgression { get { return SpellProgression.Half; } }

		public override AbilityScoreType SpellcastingAbility { get { return AbilityScoreType.Wis; } }

		private static readonly int[] m_SpellsKnown = { 0, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11 };

		public override int GetSpellsKnown(int level)
		{
			if (level < 1) return 0;
			return m_SpellsKnown[System.Math.Min(level, m_SpellsKnown.Length) - 1];
		}

		public override int SkillChoiceCount { get { return 3; } }

		public override DnDSkill[] SkillChoices
		{
			get { return new[] { DnDSkill.AnimalHandling, DnDSkill.Athletics, DnDSkill.Insight, DnDSkill.Investigation, DnDSkill.Nature, DnDSkill.Perception, DnDSkill.Stealth, DnDSkill.Survival }; }
		}
	}
}
