namespace Server.Engines.Classes
{
	public sealed class WarlockClass : CharacterClass
	{
		public override string Name { get { return "Warlock"; } }

		public override int HitDie { get { return 8; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Wis, AbilityScoreType.Cha }; }
		}

		public override WeaponCategory WeaponProficiencies
		{
			get { return WeaponCategory.SimpleMelee | WeaponCategory.SimpleRanged; }
		}

		public override ArmorCategory ArmorProficiencies { get { return ArmorCategory.Light; } }

		public override bool CanCastSpells { get { return true; } }

		public override SpellProgression SpellProgression { get { return SpellProgression.Pact; } }

		public override AbilityScoreType SpellcastingAbility { get { return AbilityScoreType.Cha; } }

		private static readonly int[] m_SpellsKnown = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15 };

		public override int GetSpellsKnown(int level)
		{
			if (level < 1) return 0;
			return m_SpellsKnown[System.Math.Min(level, m_SpellsKnown.Length) - 1];
		}

		public override int SkillChoiceCount { get { return 2; } }

		public override DnDSkill[] SkillChoices
		{
			get { return new[] { DnDSkill.Arcana, DnDSkill.Deception, DnDSkill.History, DnDSkill.Intimidation, DnDSkill.Investigation, DnDSkill.Nature, DnDSkill.Religion }; }
		}
	}
}
