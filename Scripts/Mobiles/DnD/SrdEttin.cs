namespace Server.Mobiles
{
	[CorpseName("an ettins corpse")]
	public sealed class SrdEttin : SrdMonster
	{
		[Constructable]
		public SrdEttin() : base("Ettin") { }

		public SrdEttin(Serial serial) : base(serial) { }
	}
}
