using System;

namespace Server.Items
{
	/// <summary>
	/// Base for D&amp;D weapons. Sits on Server's <see cref="Item"/> rather than ServUO's
	/// BaseWeapon, which carries slayers, runic crafting, AOS attributes, mana/skill costs and
	/// weapon abilities - none of which have a D&amp;D equivalent, and all of which drag the
	/// legacy item layer in behind them. Damage is resolved from the dice expression by the
	/// rules core in Server/.
	/// </summary>
	public abstract class DnDWeapon : Item, IDnDEquipment
	{
		public abstract WeaponCategory WeaponCategory { get; }
		public abstract string DamageDiceExpression { get; }

		public virtual ArmorCategory ArmorCategory { get { return ArmorCategory.None; } }
		public virtual int ArmorBonus { get { return 0; } }

		protected DnDWeapon(int itemID)
			: base(itemID)
		{
			Layer = Layer.OneHanded;
		}

		protected DnDWeapon(Serial serial)
			: base(serial)
		{
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1060658, "Damage\t{0}", DamageDiceExpression);
			list.Add(1060659, "Category\t{0}", WeaponCategory);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			reader.ReadInt();
		}
	}

	/// <summary>
	/// Base for D&amp;D armour. <see cref="ArmorBonus"/> is the SRD armour class the piece
	/// grants outright (not a bonus added to 10); DnDPlayerMobile applies the Dex cap by
	/// <see cref="ArmorCategory"/>.
	/// </summary>
	public abstract class DnDArmor : Item, IDnDEquipment
	{
		public abstract ArmorCategory ArmorCategory { get; }
		public abstract int ArmorBonus { get; }

		public virtual WeaponCategory WeaponCategory { get { return WeaponCategory.None; } }
		public virtual string DamageDiceExpression { get { return null; } }

		protected DnDArmor(int itemID)
			: base(itemID)
		{
			Layer = Layer.InnerTorso;
		}

		protected DnDArmor(Serial serial)
			: base(serial)
		{
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(1060658, "Armor Class\t{0}", ArmorBonus);
			list.Add(1060659, "Category\t{0}", ArmorCategory);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			reader.ReadInt();
		}
	}
}
