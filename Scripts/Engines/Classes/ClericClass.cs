namespace Server.Engines.Classes
{
	public sealed class ClericClass : CharacterClass
	{
		public override string Name { get { return "Cleric"; } }

		public override int HitDie { get { return 8; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Wis, AbilityScoreType.Cha }; }
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
	}
}
