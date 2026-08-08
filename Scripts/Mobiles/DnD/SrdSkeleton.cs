namespace Server.Mobiles
{
	[CorpseName("a skeletal corpse")]
	public sealed class SrdSkeleton : SrdMonster
	{
		[Constructable]
		public SrdSkeleton() : base("Skeleton") { }

		public SrdSkeleton(Serial serial) : base(serial) { }

	}
}
