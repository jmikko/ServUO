namespace Server.Mobiles
{
	[CorpseName("a rotting corpse")]
	public sealed class SrdZombie : SrdMonster
	{
		[Constructable]
		public SrdZombie() : base("Zombie") { }

		public SrdZombie(Serial serial) : base(serial) { }

		public override bool BleedImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lesser; } }
		public override TribeType Tribe { get { return TribeType.Undead; } }
	}
}
