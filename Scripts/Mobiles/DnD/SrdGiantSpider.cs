namespace Server.Mobiles
{
	[CorpseName("a giant spider corpse")]
	public sealed class SrdGiantSpider : SrdMonster
	{
		[Constructable]
		public SrdGiantSpider() : base("GiantSpider") { }

		public SrdGiantSpider(Serial serial) : base(serial) { }
	}
}
