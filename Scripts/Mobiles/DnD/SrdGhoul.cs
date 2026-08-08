namespace Server.Mobiles
{
	[CorpseName("a ghostly corpse")]
	public sealed class SrdGhoul : SrdMonster
	{
		[Constructable]
		public SrdGhoul() : base("Ghoul") { }

		public SrdGhoul(Serial serial) : base(serial) { }

	}
}
