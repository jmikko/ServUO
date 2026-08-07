namespace Server.Engines.Classes
{
	public sealed class MonkClass : CharacterClass
	{
		public override string Name { get { return "Monk"; } }

		public override int HitDie { get { return 8; } }

		public override AbilityScoreType[] SavingThrowProficiencies
		{
			get { return new[] { AbilityScoreType.Str, AbilityScoreType.Dex }; }
		}

		public override WeaponCategory WeaponProficiencies
		{
			get { return WeaponCategory.SimpleMelee | WeaponCategory.SimpleRanged; }
		}

		/// <summary>
		/// Monks favor Unarmored Defense over worn armor in the SRD, so no armor category is
		/// listed here - proficiency gating treats "not in the allowed set" as blocked, which is
		/// the intended behavior (a Monk in D&amp;D mode shouldn't be equipping armor at all).
		/// </summary>
		public override ArmorCategory ArmorProficiencies { get { return ArmorCategory.None; } }

		public override bool CanCastSpells { get { return false; } }
	}
}
