namespace Server.Mobiles
{
	[CorpseName("a ghostly corpse")]
	public sealed class SrdGhoul : SrdMonster
	{
		[Constructable]
		public SrdGhoul() : base("Ghoul") { }

		public SrdGhoul(Serial serial) : base(serial) { }

		public override bool BleedImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lesser; } }
		public override TribeType Tribe { get { return TribeType.Undead; } }
	}
}
