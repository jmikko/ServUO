namespace Server.Items
{
	/// <summary>
	/// Phase 1 D&amp;D 5.5e vertical slice starting-kit weapon. Subclasses the existing UO Longsword
	/// purely for art/identity; combat math under Combat.RulesMode uses the IDnDEquipment data below
	/// instead of the inherited AOS/pre-AOS damage properties (which stay untouched for legacy use).
	/// </summary>
	public class DnDLongsword : Longsword, IDnDEquipment
	{
		[Constructable]
		public DnDLongsword()
		{
		}

		public DnDLongsword(Serial serial)
			: base(serial)
		{
		}

		public WeaponCategory WeaponCategory { get { return WeaponCategory.MartialMelee; } }
		public ArmorCategory ArmorCategory { get { return ArmorCategory.None; } }
		public string DamageDiceExpression { get { return "1d8"; } }
		public int ArmorBonus { get { return 0; } }

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
