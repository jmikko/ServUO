namespace Server.Mobiles
{
	[CorpseName("a minotaur corpse")]
	public sealed class SrdMinotaur : SrdMonster
	{
		[Constructable]
		public SrdMinotaur() : base("Minotaur") { }

		public SrdMinotaur(Serial serial) : base(serial) { }
	}
}
