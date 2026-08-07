namespace Server.Engines.Races
{
	public sealed class DwarfSpecies : DnDHumanoidRace
	{
		public DwarfSpecies(int raceID, int raceIndex)
			: base(raceID, raceIndex, "Dwarf", "Dwarves")
		{
		}

		public override int GetAbilityScoreBonus(AbilityScoreType type)
		{
			return type == AbilityScoreType.Con ? 2 : 0;
		}

		public override bool HasDarkvision { get { return true; } }
	}
}
