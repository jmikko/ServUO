namespace Server.Mobiles
{
	[CorpseName("a hobgoblin corpse")]
	public sealed class SrdHobgoblin : SrdMonster
	{
		[Constructable]
		public SrdHobgoblin() : base("Hobgoblin") { }

		public SrdHobgoblin(Serial serial) : base(serial) { }
	}
}
