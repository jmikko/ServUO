namespace Server.Engines.Races
{
	public sealed class GnomeSpecies : DnDHumanoidRace
	{
		public GnomeSpecies(int raceID, int raceIndex)
			: base(raceID, raceIndex, "Gnome", "Gnomes")
		{
		}

		public override int GetAbilityScoreBonus(AbilityScoreType type)
		{
			return type == AbilityScoreType.Int ? 2 : 0;
		}

		public override bool HasDarkvision { get { return true; } }
	}
}
