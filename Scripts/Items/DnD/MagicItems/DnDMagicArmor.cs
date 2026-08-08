using System;
using Server;

namespace Server.Items
{
	public abstract class DnDMagicArmor : DnDArmor, IDnDMagicItem
	{
		public virtual bool RequiresAttunement { get { return false; } }

		public virtual int AttackBonus { get { return 0; } }
		public virtual int DamageBonus { get { return 0; } }
		public virtual int ArmorClassBonus { get { return 0; } }
		public virtual int SavingThrowBonus { get { return 0; } }

		public virtual int GetAbilityScoreOverride(AbilityScoreType type)
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

		public DnDMagicArmor(string id) : base(id)
		{
			Hue = 1150; // magical hue
		}

		public DnDMagicArmor(Serial serial) : base(serial)
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
