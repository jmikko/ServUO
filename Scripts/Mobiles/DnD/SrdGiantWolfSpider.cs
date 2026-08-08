namespace Server.Mobiles
{
	[CorpseName("a spider corpse")]
	public sealed class SrdGiantWolfSpider : SrdMonster
	{
		[Constructable]
		public SrdGiantWolfSpider() : base("GiantWolfSpider") { }

		public SrdGiantWolfSpider(Serial serial) : base(serial) { }
	}
}
