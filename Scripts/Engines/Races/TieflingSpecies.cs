namespace Server.Engines.Races
{
	public sealed class TieflingSpecies : DnDHumanoidRace
	{
		public TieflingSpecies(int raceID, int raceIndex)
			: base(raceID, raceIndex, "Tiefling", "Tieflings")
		{
		}

		public override int GetAbilityScoreBonus(AbilityScoreType type)
		{
			if (type == AbilityScoreType.Cha) { return 2; }
			if (type == AbilityScoreType.Int) { return 1; }
			return 0;
		}

		public override bool HasDarkvision { get { return true; } }
	}
}
