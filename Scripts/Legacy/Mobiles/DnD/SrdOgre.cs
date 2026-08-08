namespace Server.Mobiles
{
	[CorpseName("an ogre corpse")]
	public sealed class SrdOgre : SrdMonster
	{
		[Constructable]
		public SrdOgre() : base("Ogre") { }

		public SrdOgre(Serial serial) : base(serial) { }
	}
}
