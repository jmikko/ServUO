namespace Server.Mobiles
{
	[CorpseName("a skeletal corpse")]
	public sealed class SrdSkeleton : SrdMonster
	{
		[Constructable]
		public SrdSkeleton() : base("Skeleton") { }

		public SrdSkeleton(Serial serial) : base(serial) { }

		public override bool BleedImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lesser; } }
		public override TribeType Tribe { get { return TribeType.Undead; } }
	}
}
