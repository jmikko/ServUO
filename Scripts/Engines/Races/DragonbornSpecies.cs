namespace Server.Engines.Races
{
	public sealed class DragonbornSpecies : DnDHumanoidRace
	{
		public DragonbornSpecies(int raceID, int raceIndex)
			: base(raceID, raceIndex, "Dragonborn", "Dragonborn")
		{
		}

		public override int GetAbilityScoreBonus(AbilityScoreType type)
		{
			if (type == AbilityScoreType.Str) { return 2; }
			if (type == AbilityScoreType.Cha) { return 1; }
			return 0;
		}

		public override bool HasDarkvision { get { return false; } }
	}
}
