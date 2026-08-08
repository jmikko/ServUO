namespace Server.Engines.Classes
{
	public sealed class RogueClass : CharacterClass
	{
		public override string Name { get { return "Rogue"; } }

		public override int HitDie { get { return 8; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Dex, AbilityScoreType.Int }; }
		}

		public override WeaponCategory WeaponProficiencies
		{
			get { return WeaponCategory.SimpleMelee | WeaponCategory.SimpleRanged; }
		}

		public override ArmorCategory ArmorProficiencies { get { return ArmorCategory.Light; } }

		public override bool CanCastSpells { get { return false; } }

		public override int GetAbilityScoreImprovements(int level)
		{
			if (level == 4 || level == 8 || level == 10 || level == 12 || level == 16 || level == 19)
			{
				return 2;
			}
			return 0;
		}

		public override int SkillChoiceCount { get { return 4; } }

		public override DnDSkill[] SkillChoices
		{
			get { return new[] { DnDSkill.Acrobatics, DnDSkill.Athletics, DnDSkill.Deception, DnDSkill.Insight, DnDSkill.Intimidation, DnDSkill.Investigation, DnDSkill.Perception, DnDSkill.Performance, DnDSkill.Persuasion, DnDSkill.SleightOfHand, DnDSkill.Stealth }; }
		}
	}
}
