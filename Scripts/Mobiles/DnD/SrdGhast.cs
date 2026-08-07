namespace Server.Mobiles
{
	[CorpseName("a ghastly corpse")]
	public sealed class SrdGhast : SrdMonster
	{
		[Constructable]
		public SrdGhast() : base("Ghast") { }

		public SrdGhast(Serial serial) : base(serial) { }
	}
}
