using System;

namespace Server.Items
{
	/// <summary>
	/// Base for D&amp;D weapons. Sits on Server's <see cref="Item"/> rather than ServUO's
	/// BaseWeapon, which carries slayers, runic crafting, AOS attributes, mana/skill costs and
	/// weapon abilities - none of which have a D&amp;D equivalent, and all of which drag the
	/// legacy item layer in behind them.
	/// <para>
	/// Every stat comes from Data/DnDWeapons.xml via <see cref="DnDEquipmentTable"/>; a concrete
	/// subclass supplies only its id.
	/// </para>
	/// </summary>
	public abstract class DnDWeapon : Item, IDnDEquipment, IWeapon
	{
		public abstract string WeaponId { get; }

		public DnDWeaponData Data { get { return DnDEquipmentTable.GetWeapon(WeaponId); } }

		public WeaponCategory WeaponCategory { get { return Data.Category; } }
		public WeaponProperty Properties { get { return Data.Properties; } }
		public DamageType DamageType { get { return Data.DamageType; } }

		public virtual ArmorCategory ArmorCategory { get { return ArmorCategory.None; } }
		public virtual int ArmorBonus { get { return 0; } }

		public bool IsFinesse { get { return Data.Has(WeaponProperty.Finesse); } }

		public bool IsRanged
		{
			get
			{
				WeaponCategory category = Data.Category;

				return category == WeaponCategory.SimpleRanged || category == WeaponCategory.MartialRanged;
			}
		}

		/// <summary>
		/// A versatile weapon rolls its larger die when nothing occupies the other hand. Two-handed
		/// weapons already occupy both, so they never take this path.
		/// </summary>
		public string DamageDiceExpression
		{
			get
			{
				DnDWeaponData data = Data;

				if (!data.Has(WeaponProperty.Versatile) || String.IsNullOrEmpty(data.VersatileDamage))
				{
					return data.Damage;
				}

				return IsWieldedTwoHanded() ? data.VersatileDamage : data.Damage;
			}
		}

		private bool IsWieldedTwoHanded()
		{
			Mobile holder = Parent as Mobile;

			if (holder == null)
			{
				return false;
			}

			// Versatile means "two-handed if the other hand is free".
			return holder.FindItemOnLayer(Layer.TwoHanded) == null;
		}

		/// <summary>Reach adds a tile; ranged weapons carry their own range from the table.</summary>
		public virtual int MaxRange
		{
			get
			{
				DnDWeaponData data = Data;

				if (IsRanged)
				{
					return Math.Max(1, data.Range);
				}

				return data.Has(WeaponProperty.Reach) ? 2 : 1;
			}
		}

		protected DnDWeapon(string id)
			: base(DnDEquipmentTable.GetWeapon(id).ItemID)
		{
			DnDWeaponData data = DnDEquipmentTable.GetWeapon(id);

			Name = data.Name;
			Weight = data.Weight;
			Layer = data.Layer;
		}

		protected DnDWeapon(Serial serial)
			: base(serial)
		{
		}

		public virtual void OnBeforeSwing(Mobile attacker, IDamageable damageable)
		{ }

		public virtual TimeSpan OnSwing(Mobile attacker, IDamageable damageable)
		{
			return DnDCombat.Resolve(attacker, damageable, this);
		}

		/// <summary>Feeds the client's paperdoll damage range.</summary>
		public virtual void GetStatusDamage(Mobile from, out int min, out int max)
		{
			CombatRules.GetDiceRange(DamageDiceExpression, out min, out max);

			int bonus = CombatRules.GetDamageBonus(from, IsRanged, IsFinesse);

			min += bonus;
			max += bonus;
		}

		public virtual TimeSpan GetDelay(Mobile attacker)
		{
			return DnDCombat.SwingDelay;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			DnDWeaponData data = Data;

			list.Add(1060658, "Damage\t{0} {1}", DamageDiceExpression, data.DamageType);
			list.Add(1060659, "Category\t{0}", SplitCamelCase(data.Category.ToString()));

			if (data.Properties != WeaponProperty.None)
			{
				list.Add(1060660, "Properties\t{0}", SplitCamelCase(data.Properties.ToString()));
			}
		}

		/// <summary>"MartialMelee" and "Finesse, Light" both want spaces the enum names lack.</summary>
		internal static string SplitCamelCase(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				return value;
			}

			var builder = new System.Text.StringBuilder(value.Length + 8);

			for (int i = 0; i < value.Length; ++i)
			{
				if (i > 0 && Char.IsUpper(value[i]) && value[i - 1] != ' ' && value[i - 1] != ',')
				{
					builder.Append(' ');
				}

				builder.Append(value[i]);
			}

			return builder.ToString();
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
	/// Base for D&amp;D armour. <see cref="ArmorBonus"/> is the SRD armour class the piece grants
	/// outright (not a bonus added to 10), except for shields, which add theirs on top.
	/// </summary>
	public abstract class DnDArmor : Item, IDnDEquipment
	{
		public abstract string ArmorId { get; }

		public DnDArmorData Data { get { return DnDEquipmentTable.GetArmor(ArmorId); } }

		public ArmorCategory ArmorCategory { get { return Data.Category; } }
		public int ArmorBonus { get { return Data.BaseArmorClass; } }

		/// <summary>-1 when the wearer's full Dexterity modifier applies.</summary>
		public int MaxDexBonus { get { return Data.MaxDexBonus; } }

		public int MinimumStrength { get { return Data.MinimumStrength; } }
		public bool StealthDisadvantage { get { return Data.StealthDisadvantage; } }

		public WeaponCategory WeaponCategory { get { return WeaponCategory.None; } }
		public string DamageDiceExpression { get { return null; } }
		public bool IsFinesse { get { return false; } }

		protected DnDArmor(string id)
			: base(DnDEquipmentTable.GetArmor(id).ItemID)
		{
			DnDArmorData data = DnDEquipmentTable.GetArmor(id);

			Name = data.Name;
			Weight = data.Weight;
			Layer = data.Layer;
		}

		protected DnDArmor(Serial serial)
			: base(serial)
		{
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			DnDArmorData data = Data;

			if (data.Category == ArmorCategory.Shield)
			{
				list.Add(1060658, "Armor Class\t+{0}", data.BaseArmorClass);
			}
			else
			{
				list.Add(1060658, "Armor Class\t{0}", data.BaseArmorClass);
			}

			list.Add(1060659, "Category\t{0}", data.Category);

			if (data.MinimumStrength > 0)
			{
				list.Add(1060660, "Requires\tStrength {0}", data.MinimumStrength);
			}

			if (data.StealthDisadvantage)
			{
				list.Add(1060661, "Stealth\tDisadvantage");
			}
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
