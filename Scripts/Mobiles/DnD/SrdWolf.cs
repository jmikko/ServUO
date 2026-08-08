namespace Server.Mobiles
{
	[CorpseName("a wolf corpse")]
	public sealed class SrdWolf : SrdMonster
	{
		[Constructable]
		public SrdWolf() : base("Wolf") { }

		public SrdWolf(Serial serial) : base(serial) { }

	}
}
