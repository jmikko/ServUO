namespace Server.Engines.Races
{
	public sealed class HalfOrcSpecies : DnDHumanoidRace
	{
		public HalfOrcSpecies(int raceID, int raceIndex)
			: base(raceID, raceIndex, "Half-Orc", "Half-Orcs")
		{
		}

		public override int GetAbilityScoreBonus(AbilityScoreType type)
		{
			if (type == AbilityScoreType.Str) { return 2; }
			if (type == AbilityScoreType.Con) { return 1; }
			return 0;
		}

		public override bool HasDarkvision { get { return true; } }
	}
}
