namespace Server.Engines.Races
{
	public sealed class HalflingSpecies : DnDHumanoidRace
	{
		public HalflingSpecies(int raceID, int raceIndex)
			: base(raceID, raceIndex, "Halfling", "Halflings")
		{
		}

		public override int GetAbilityScoreBonus(AbilityScoreType type)
		{
			return type == AbilityScoreType.Dex ? 2 : 0;
		}

		public override bool HasDarkvision { get { return false; } }
	}
}
