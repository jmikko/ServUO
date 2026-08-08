namespace Server.Items
{
	/// <summary>SRD leather armour: light armour, AC 11 plus the full Dexterity modifier.</summary>
	public sealed class DnDLeatherArmor : DnDArmor
	{
		public override ArmorCategory ArmorCategory { get { return ArmorCategory.Light; } }
		public override int ArmorBonus { get { return 11; } }

		[Constructable]
		public DnDLeatherArmor() : base(0x13CC)
		{
			Name = "leather armour";
			Weight = 10.0;
		}

		public DnDLeatherArmor(Serial serial) : base(serial) { }
	}
}
