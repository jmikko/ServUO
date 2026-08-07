namespace Server.Mobiles
{
	[CorpseName("a gnoll corpse")]
	public sealed class SrdGnoll : SrdMonster
	{
		[Constructable]
		public SrdGnoll() : base("Gnoll") { }

		public SrdGnoll(Serial serial) : base(serial) { }
	}
}
