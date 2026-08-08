namespace Server.Engines.Classes
{
	public sealed class SorcererClass : CharacterClass
	{
		public override string Name { get { return "Sorcerer"; } }

		public override int HitDie { get { return 6; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Con, AbilityScoreType.Cha }; }
		}

		public override WeaponCategory WeaponProficiencies
		{
			get { return WeaponCategory.SimpleMelee | WeaponCategory.SimpleRanged; }
		}

		public override ArmorCategory ArmorProficiencies { get { return ArmorCategory.None; } }

		public override bool CanCastSpells { get { return true; } }

		public override SpellProgression SpellProgression { get { return SpellProgression.Full; } }

		public override AbilityScoreType SpellcastingAbility { get { return AbilityScoreType.Cha; } }
	}
}
