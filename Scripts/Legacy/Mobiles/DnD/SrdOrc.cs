namespace Server.Mobiles
{
	[CorpseName("an orcish corpse")]
	public sealed class SrdOrc : SrdMonster
	{
		[Constructable]
		public SrdOrc() : base("Orc") { }

		public SrdOrc(Serial serial) : base(serial) { }
	}
}
