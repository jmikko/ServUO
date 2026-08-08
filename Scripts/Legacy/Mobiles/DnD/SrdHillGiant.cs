namespace Server.Mobiles
{
	[CorpseName("a giant corpse")]
	public sealed class SrdHillGiant : SrdMonster
	{
		[Constructable]
		public SrdHillGiant() : base("HillGiant") { }

		public SrdHillGiant(Serial serial) : base(serial) { }
	}
}
