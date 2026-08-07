namespace Server.Mobiles
{
	[CorpseName("a harpy corpse")]
	public sealed class SrdHarpy : SrdMonster
	{
		[Constructable]
		public SrdHarpy() : base("Harpy") { }

		public SrdHarpy(Serial serial) : base(serial) { }
	}
}
