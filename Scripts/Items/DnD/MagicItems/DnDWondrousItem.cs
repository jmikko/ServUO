using System;
using System.Text;

namespace Server.Items
{
	/// <summary>
	/// One wondrous item, driven entirely by its row in Data/DnDMagicItems.xml.
	/// <para>
	/// The row id is serialised rather than the numbers it resolves to, so retuning an item in the
	/// table retunes every copy already in the world - the same choice the weapon and armour tables
	/// make, and the reason a balance pass does not mean a save migration.
	/// </para>
	/// </summary>
	public abstract class DnDWondrousItem : Item, IDnDMagicItem
	{
		private string m_ItemId;

		public abstract string WondrousId { get; }

		public DnDWondrousData Data { get { return DnDWondrousTable.Get(m_ItemId ?? WondrousId); } }

		public bool RequiresAttunement { get { return Data.RequiresAttunement; } }

		public int AttackBonus { get { return Data.AttackBonus; } }
		public int DamageBonus { get { return Data.DamageBonus; } }
		public int ArmorClassBonus { get { return Data.ArmorClassBonus; } }
		public int SavingThrowBonus { get { return Data.SavingThrowBonus; } }

		public int GetAbilityScoreOverride(AbilityScoreType type)
		{
			return Data.AbilityOverrides[(int)type];
		}

		public DnDWondrousItem(string id)
			: base(DnDWondrousTable.Get(id).ItemID)
		{
			DnDWondrousData data = DnDWondrousTable.Get(id);

			m_ItemId = id;

			Name = data.Name;
			Layer = data.Layer;
			Hue = data.Hue;
			Weight = data.Weight;
		}

		public DnDWondrousItem(Serial serial)
			: base(serial)
		{
		}

		/// <summary>
		/// Says what the item does, on the item. A magic item nobody can read the effect of is a
		/// hue and a name, and the whole point of the table is that the numbers are knowable.
		/// </summary>
		public override void AddNameProperty(ObjectPropertyList list)
		{
			base.AddNameProperty(list);

			DnDWondrousData data = Data;

			var effects = new StringBuilder();

			Append(effects, data.ArmorClassBonus, "Armor Class");
			Append(effects, data.AttackBonus, "attack rolls");
			Append(effects, data.DamageBonus, "damage");
			Append(effects, data.SavingThrowBonus, "saving throws");

			for (int i = 0; i < data.AbilityOverrides.Length; ++i)
			{
				if (data.AbilityOverrides[i] > 0)
				{
					if (effects.Length > 0)
					{
						effects.Append(", ");
					}

					effects.AppendFormat("{0} becomes {1}", (AbilityScoreType)i, data.AbilityOverrides[i]);
				}
			}

			if (effects.Length > 0)
			{
				list.Add(1049644, effects.ToString());
			}

			if (data.RequiresAttunement)
			{
				list.Add(1049644, "Requires Attunement");
			}
		}

		private static void Append(StringBuilder builder, int bonus, string what)
		{
			if (bonus == 0)
			{
				return;
			}

			if (builder.Length > 0)
			{
				builder.Append(", ");
			}

			builder.AppendFormat("{0}{1} {2}", bonus > 0 ? "+" : "", bonus, what);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.Write(m_ItemId);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();

			m_ItemId = reader.ReadString();
		}
	}
}
