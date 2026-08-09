using System;
using Server;

namespace Server.Items
{
	public abstract class DnDMagicWeapon : DnDWeapon, IDnDMagicItem
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
		/// Called by the combat resolver once a hit has landed and its damage been applied.
		/// <para>
		/// This is where a weapon whose effect is not a flat bonus does its work - a Dagger of
		/// Venom poisoning, a Sword of Life Stealing healing its wielder. The bonuses above are
		/// read before the roll; this is read after it, and gets told whether the hit was a
		/// critical, because a good many SRD weapons only trigger on one.
		/// </para>
		/// </summary>
		public virtual void OnHit(Mobile attacker, IDamageable defender, int damage, bool critical)
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

		public DnDMagicWeapon(string name) : base(name)
		{
			Hue = 1150; // magical hue
		}

		public DnDMagicWeapon(Serial serial) : base(serial)
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
