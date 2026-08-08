namespace Server.Engines.Classes
{
	public class DruidClass : CharacterClass
	{
		public override string Name { get { return "Druid"; } }

		public override int HitDie { get { return 8; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Int, AbilityScoreType.Wis }; }
		}

		public override WeaponCategory WeaponProficiencies
		{
			get { return WeaponCategory.SimpleMelee | WeaponCategory.SimpleRanged; }
		}

		public override ArmorCategory ArmorProficiencies
		{
			get { return ArmorCategory.Light | ArmorCategory.Medium | ArmorCategory.Shield; }
		}

		public override bool CanCastSpells { get { return true; } }

		public override SpellProgression SpellProgression { get { return SpellProgression.Full; } }

		public override AbilityScoreType SpellcastingAbility { get { return AbilityScoreType.Wis; } }

		public override int GetSpellsKnown(int level)
		{
			return int.MaxValue;
		}

		public override int SkillChoiceCount { get { return 2; } }

		public override DnDSkill[] SkillChoices
		{
			get { return new[] { DnDSkill.Arcana, DnDSkill.AnimalHandling, DnDSkill.Insight, DnDSkill.Medicine, DnDSkill.Nature, DnDSkill.Perception, DnDSkill.Religion, DnDSkill.Survival }; }
		}
	}
}
