namespace Server.Mobiles
{
	[CorpseName("a bloated corpse")]
	public sealed class SrdOgreZombie : SrdMonster
	{
		[Constructable]
		public SrdOgreZombie() : base("OgreZombie") { }

		public SrdOgreZombie(Serial serial) : base(serial) { }
	}
}
