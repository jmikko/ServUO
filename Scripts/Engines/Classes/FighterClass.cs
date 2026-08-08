namespace Server.Engines.Classes
{
	public sealed class FighterClass : CharacterClass
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
	}
}
