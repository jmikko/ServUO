namespace Server.Engines.Classes
{
	public sealed class BarbarianClass : CharacterClass
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
	}
}
