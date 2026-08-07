namespace Server.Mobiles
{
	[CorpseName("a wolf corpse")]
	public sealed class SrdWolf : SrdMonster
	{
		[Constructable]
		public SrdWolf() : base("Wolf") { }

		public SrdWolf(Serial serial) : base(serial) { }

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 6; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Canine; } }
	}
}
