namespace Server.Engines.Classes
{
	public class PaladinClass : CharacterClass
	{
		public override string Name { get { return "Paladin"; } }

		public override int HitDie { get { return 10; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Wis, AbilityScoreType.Cha }; }
		}

		public override WeaponCategory WeaponProficiencies { get { return WeaponCategory.All; } }

		public override ArmorCategory ArmorProficiencies { get { return ArmorCategory.All; } }

		public override bool CanCastSpells { get { return true; } }

		public override SpellProgression SpellProgression { get { return SpellProgression.Half; } }

		public override AbilityScoreType SpellcastingAbility { get { return AbilityScoreType.Cha; } }

		public override int GetSpellsKnown(int level)
		{
			return int.MaxValue;
		}

		public override int SkillChoiceCount { get { return 2; } }

		public override DnDSkill[] SkillChoices
		{
			get { return new[] { DnDSkill.Athletics, DnDSkill.Insight, DnDSkill.Intimidation, DnDSkill.Medicine, DnDSkill.Persuasion, DnDSkill.Religion }; }
		}
	}
}
