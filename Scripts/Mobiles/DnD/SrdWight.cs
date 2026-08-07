namespace Server.Mobiles
{
	[CorpseName("a wight corpse")]
	public sealed class SrdWight : SrdMonster
	{
		[Constructable]
		public SrdWight() : base("Wight") { }

		public SrdWight(Serial serial) : base(serial) { }

		public override TribeType Tribe { get { return TribeType.Undead; } }
	}
}
