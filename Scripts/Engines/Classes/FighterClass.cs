namespace Server.Engines.Classes
{
	public class FighterClass : CharacterClass
	{
		public override string Name { get { return "Fighter"; } }

		public override int HitDie { get { return 10; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Str, AbilityScoreType.Con }; }
		}

		public override WeaponCategory WeaponProficiencies { get { return WeaponCategory.All; } }

		public override ArmorCategory ArmorProficiencies { get { return ArmorCategory.All; } }

		public override bool CanCastSpells { get { return false; } }

		public override int GetAbilityScoreImprovements(int level)
		{
			if (level == 4 || level == 6 || level == 8 || level == 12 || level == 14 || level == 16 || level == 19)
			{
				return 2;
			}
			return 0;
		}

		public override int SkillChoiceCount { get { return 2; } }

		public override DnDSkill[] SkillChoices
		{
			get { return new[] { DnDSkill.Acrobatics, DnDSkill.AnimalHandling, DnDSkill.Athletics, DnDSkill.History, DnDSkill.Insight, DnDSkill.Intimidation, DnDSkill.Perception, DnDSkill.Survival }; }
		}
	}
}
