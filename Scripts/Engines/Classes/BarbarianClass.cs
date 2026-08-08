namespace Server.Engines.Classes
{
	public class BarbarianClass : CharacterClass
	{
		public override string Name { get { return "Barbarian"; } }

		public override int HitDie { get { return 12; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Str, AbilityScoreType.Con }; }
		}

		public override WeaponCategory WeaponProficiencies { get { return WeaponCategory.All; } }

		public override ArmorCategory ArmorProficiencies
		{
			get { return ArmorCategory.Light | ArmorCategory.Medium | ArmorCategory.Shield; }
		}

		public override bool CanCastSpells { get { return false; } }

		public override int SkillChoiceCount { get { return 2; } }

		public override DnDSkill[] SkillChoices
		{
			get { return new[] { DnDSkill.AnimalHandling, DnDSkill.Athletics, DnDSkill.Intimidation, DnDSkill.Nature, DnDSkill.Perception, DnDSkill.Survival }; }
		}


		public override ClassFeature[] Features
		{
			get { return new ClassFeature[] { new Features.RageFeature(), new Features.BarbarianUnarmoredDefenseFeature(), new Features.DangerSenseFeature(), new Features.ExtraAttackFeature(), new Features.BrutalCriticalFeature(), new Features.PrimalChampionFeature() }; }
		}
	}
}
