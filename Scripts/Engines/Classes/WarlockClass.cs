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
	}
}
