namespace Server.Mobiles
{
	[CorpseName("a ghostly residue")]
	public sealed class SrdSpecter : SrdMonster
	{
		[Constructable]
		public SrdSpecter() : base("Specter") { }

		public SrdSpecter(Serial serial) : base(serial) { }
	}
}
