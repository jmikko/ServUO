namespace Server.Mobiles
{
	[CorpseName("a winter wolf corpse")]
	public sealed class SrdWinterWolf : SrdMonster
	{
		[Constructable]
		public SrdWinterWolf() : base("WinterWolf") { }

		public SrdWinterWolf(Serial serial) : base(serial) { }
	}
}
