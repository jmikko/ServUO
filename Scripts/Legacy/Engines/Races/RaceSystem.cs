namespace Server.Engines.Races
{
	public static class RaceSystem
	{
		// IDs 0-31 and 0x7F/0xFF are reserved for core use (see Scripts/Misc/RaceDefinitions.cs);
		// new D&D-only species start at 32.
		public static void Configure()
		{
			RegisterRace(new DwarfSpecies(32, 32));
			RegisterRace(new HalflingSpecies(33, 33));
			RegisterRace(new GnomeSpecies(34, 34));
			RegisterRace(new HalfOrcSpecies(35, 35));
			RegisterRace(new TieflingSpecies(36, 36));
			RegisterRace(new DragonbornSpecies(37, 37));
		}

		private static void RegisterRace(Race race)
		{
			Race.Races[race.RaceIndex] = race;
			Race.AllRaces.Add(race);
		}
	}
}
