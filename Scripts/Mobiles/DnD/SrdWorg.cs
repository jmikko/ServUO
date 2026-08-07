namespace Server.Mobiles
{
	[CorpseName("a worg corpse")]
	public sealed class SrdWorg : SrdMonster
	{
		[Constructable]
		public SrdWorg() : base("Worg") { }

		public SrdWorg(Serial serial) : base(serial) { }
	}
}
