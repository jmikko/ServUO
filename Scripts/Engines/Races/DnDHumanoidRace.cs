namespace Server.Engines.Races
{
	/// <summary>
	/// Shared base for new D&amp;D 5.5e species that have no unique UO art (SRD species like Dwarf,
	/// Halfling, Gnome, Half-Orc, Tiefling, and Dragonborn don't exist as UO body types). Per the
	/// project's "reuse existing UO art" decision, these reuse Human's body graphics and delegate
	/// every appearance-validation method to the real Race.Human instance, so only the constructor
	/// and the two IDnDSpecies members need writing per new species instead of the full ~12-method
	/// abstract Race surface.
	/// </summary>
	public abstract class DnDHumanoidRace : Race, IDnDSpecies
	{
		protected DnDHumanoidRace(int raceID, int raceIndex, string name, string pluralName)
			: base(raceID, raceIndex, name, pluralName, 400, 401, 402, 403, Expansion.None)
		{
		}

		public override bool ValidateHair(bool female, int itemID) { return Race.Human.ValidateHair(female, itemID); }
		public override int RandomHair(bool female) { return Race.Human.RandomHair(female); }
		public override bool ValidateFacialHair(bool female, int itemID) { return Race.Human.ValidateFacialHair(female, itemID); }
		public override int RandomFacialHair(bool female) { return Race.Human.RandomFacialHair(female); }
		public override bool ValidateFace(bool female, int itemID) { return Race.Human.ValidateFace(female, itemID); }
		public override int RandomFace(bool female) { return Race.Human.RandomFace(female); }
		public override int ClipSkinHue(int hue) { return Race.Human.ClipSkinHue(hue); }
		public override int RandomSkinHue() { return Race.Human.RandomSkinHue(); }
		public override int ClipHairHue(int hue) { return Race.Human.ClipHairHue(hue); }
		public override int RandomHairHue() { return Race.Human.RandomHairHue(); }
		public override int ClipFaceHue(int hue) { return Race.Human.ClipFaceHue(hue); }
		public override int RandomFaceHue() { return Race.Human.RandomFaceHue(); }

		public abstract int GetAbilityScoreBonus(AbilityScoreType type);
		public abstract bool HasDarkvision { get; }
	}
}
