namespace Server.Mobiles
{
	[CorpseName("a scorpion corpse")]
	public sealed class SrdGiantScorpion : SrdMonster
	{
		[Constructable]
		public SrdGiantScorpion() : base("GiantScorpion") { }

		public SrdGiantScorpion(Serial serial) : base(serial) { }
	}
}
