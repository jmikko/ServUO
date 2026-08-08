namespace Server.Engines.Classes
{
	public sealed class RangerClass : CharacterClass
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
	}
}
