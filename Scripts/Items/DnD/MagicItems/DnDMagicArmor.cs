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

		/// <summary>
		/// Called for every worn piece before the damage of a hit is applied, and able to change it.
		/// <para>
		/// Armour that grants resistance, absorbs a fixed amount, or turns a critical back into an
		/// ordinary hit works here rather than through the flat bonuses above, which are read
		/// before the attack roll and cannot see what the hit turned out to be. The damage is
		/// passed by reference because reducing it is the whole point; a piece that only wants to
		/// react - a shield that shouts, armour that sheds a charge - can ignore it.
		/// </para>
		/// </summary>
		public virtual void OnTakeDamage(Mobile attacker, Mobile defender, ref int damage, bool critical)
		{
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
