namespace Server.Mobiles
{
	[CorpseName("a troll corpse")]
	public sealed class SrdTroll : SrdMonster
	{
		[Constructable]
		public SrdTroll() : base("Troll") { }

		public SrdTroll(Serial serial) : base(serial) { }
	}
}
