namespace Server.Engines.Classes
{
	public sealed class WizardClass : CharacterClass
	{
		public override string Name { get { return "Wizard"; } }

		public override int HitDie { get { return 6; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Int, AbilityScoreType.Wis }; }
		}

		public override WeaponCategory WeaponProficiencies
		{
			get { return WeaponCategory.SimpleMelee | WeaponCategory.SimpleRanged; }
		}

		public override ArmorCategory ArmorProficiencies { get { return ArmorCategory.None; } }

		public override bool CanCastSpells { get { return true; } }
	}
}
