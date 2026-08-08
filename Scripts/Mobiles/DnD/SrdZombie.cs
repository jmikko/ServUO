namespace Server.Mobiles
{
	[CorpseName("a rotting corpse")]
	public sealed class SrdZombie : SrdMonster
	{
		[Constructable]
		public SrdZombie() : base("Zombie") { }

		public SrdZombie(Serial serial) : base(serial) { }

	}
}
