namespace Server.Mobiles
{
	[CorpseName("a goblin corpse")]
	public sealed class SrdGoblin : SrdMonster
	{
		[Constructable]
		public SrdGoblin() : base("Goblin") { }

		public SrdGoblin(Serial serial) : base(serial) { }
	}
}
