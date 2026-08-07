namespace Server.Mobiles
{
	[CorpseName("a bugbear corpse")]
	public sealed class SrdBugbear : SrdMonster
	{
		[Constructable]
		public SrdBugbear() : base("Bugbear") { }

		public SrdBugbear(Serial serial) : base(serial) { }
	}
}
