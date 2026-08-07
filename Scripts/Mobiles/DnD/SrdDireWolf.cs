namespace Server.Mobiles
{
	[CorpseName("a dire wolf corpse")]
	public sealed class SrdDireWolf : SrdMonster
	{
		[Constructable]
		public SrdDireWolf() : base("DireWolf") { }

		public SrdDireWolf(Serial serial) : base(serial) { }
	}
}
