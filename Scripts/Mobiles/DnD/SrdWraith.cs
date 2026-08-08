namespace Server.Mobiles
{
	[CorpseName("a wraiths corpse")]
	public sealed class SrdWraith : SrdMonster
	{
		[Constructable]
		public SrdWraith() : base("Wraith") { }

		public SrdWraith(Serial serial) : base(serial) { }
	}
}
