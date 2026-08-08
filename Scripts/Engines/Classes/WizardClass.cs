namespace Server.Engines.Classes
{
	public class WizardClass : CharacterClass
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

		public override SpellProgression SpellProgression { get { return SpellProgression.Full; } }

		public override AbilityScoreType SpellcastingAbility { get { return AbilityScoreType.Int; } }

		public override int GetSpellsKnown(int level)
		{
			if (level < 1) return 0;
			return 6 + (level - 1) * 2;
		}
	}
}
