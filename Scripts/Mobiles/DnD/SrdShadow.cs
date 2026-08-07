namespace Server.Mobiles
{
	[CorpseName("a pool of darkness")]
	public sealed class SrdShadow : SrdMonster
	{
		[Constructable]
		public SrdShadow() : base("Shadow") { }

		public SrdShadow(Serial serial) : base(serial) { }
	}
}
