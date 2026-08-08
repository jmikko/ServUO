namespace Server.Mobiles
{
	[CorpseName("a goblin corpse")]
	public sealed class SrdGoblinBoss : SrdMonster
	{
		[Constructable]
		public SrdGoblinBoss() : base("GoblinBoss") { }

		public SrdGoblinBoss(Serial serial) : base(serial) { }
	}
}
