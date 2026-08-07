namespace Server.Items
{
	/// <summary>
	/// Phase 1 D&amp;D 5.5e vertical slice starting-kit armor. Subclasses the existing UO ChainChest
	/// purely for art/identity; PlayerMobile.ArmorClass uses the IDnDEquipment data below (SRD chain
	/// shirt: base AC 13, Dex modifier applies up to +2) instead of the inherited UO ArmorRating.
	/// </summary>
	public class DnDChainShirt : ChainChest, IDnDEquipment
	{
		[Constructable]
		public DnDChainShirt()
		{
		}

		public DnDChainShirt(Serial serial)
			: base(serial)
		{
		}

		public WeaponCategory WeaponCategory { get { return WeaponCategory.None; } }
		public ArmorCategory ArmorCategory { get { return ArmorCategory.Medium; } }
		public string DamageDiceExpression { get { return null; } }
		public int ArmorBonus { get { return 13; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
