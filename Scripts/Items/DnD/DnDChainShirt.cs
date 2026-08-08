namespace Server.Items
{
	/// <summary>SRD chain shirt: medium armour, AC 13 (+ Dex, capped at +2).</summary>
	public class DnDChainShirt : DnDArmor
	{
		public override ArmorCategory ArmorCategory { get { return ArmorCategory.Medium; } }
		public override int ArmorBonus { get { return 13; } }

		[Constructable]
		public DnDChainShirt()
			: base(0x13BF)
		{
			Name = "a chain shirt";
			Weight = 20.0;
		}

		public DnDChainShirt(Serial serial)
			: base(serial)
		{
		}
	}
}
