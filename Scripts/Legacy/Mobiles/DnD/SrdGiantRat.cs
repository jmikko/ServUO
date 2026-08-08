namespace Server.Mobiles
{
	[CorpseName("a giant rat corpse")]
	public sealed class SrdGiantRat : SrdMonster
	{
		[Constructable]
		public SrdGiantRat() : base("GiantRat") { }

		public SrdGiantRat(Serial serial) : base(serial) { }
	}
}
