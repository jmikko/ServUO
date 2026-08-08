using System;
using Server;

namespace Server.Items
{
	public class DnDRingOfProtection : Item, IDnDMagicItem
	{
		public bool RequiresAttunement { get { return true; } }

		public int AttackBonus { get { return 0; } }
		public int DamageBonus { get { return 0; } }
		public int ArmorClassBonus { get { return 1; } }
		public int SavingThrowBonus { get { return 1; } }

		public int GetAbilityScoreOverride(AbilityScoreType type)
		{
			return 0;
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (RequiresAttunement)
			{
				list.Add(1049644, "Requires Attunement"); // [Requires Attunement]
			}
			base.AddNameProperty(list);
		}

		[Constructable]
		public DnDRingOfProtection() : base(0x108a) // Ring
		{
			Name = "Ring of Protection";
			Weight = 0.1;
			Hue = 1150;
			Layer = Layer.Ring;
		}

		public DnDRingOfProtection(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class DnDAmuletOfHealth : Item, IDnDMagicItem
	{
		public bool RequiresAttunement { get { return true; } }

		public int AttackBonus { get { return 0; } }
		public int DamageBonus { get { return 0; } }
		public int ArmorClassBonus { get { return 0; } }
		public int SavingThrowBonus { get { return 0; } }

		public int GetAbilityScoreOverride(AbilityScoreType type)
		{
			if (type == AbilityScoreType.Con)
			{
				return 19;
			}
			return 0;
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (RequiresAttunement)
			{
				list.Add(1049644, "Requires Attunement"); // [Requires Attunement]
			}
			base.AddNameProperty(list);
		}

		[Constructable]
		public DnDAmuletOfHealth() : base(0x1088) // Necklace
		{
			Name = "Amulet of Health";
			Weight = 1.0;
			Hue = 1150;
			Layer = Layer.Neck;
		}

		public DnDAmuletOfHealth(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
