using System;

namespace Server.Items
{
	/// <summary>
	/// The named magic weapons, built on the weapon table rather than the wondrous one.
	/// <para>
	/// These were rows in Data/DnDMagicItems.xml sitting on weapon layers as plain items. Their
	/// bonuses applied, so they looked like they worked - but the combat resolver reads damage dice
	/// through <see cref="IDnDEquipment"/>, and a plain <c>Item</c> is not one, so a character
	/// holding a Vorpal Sword swung as if unarmed for 1d1. The self-test named each of them at boot
	/// for exactly this reason.
	/// </para>
	/// <para>
	/// Each carries a base weapon id from the weapon table, so a Flame Tongue is a longsword that
	/// also burns rather than a separate thing that happens to be sword-shaped. The magical extras
	/// stay as properties here, because they are behaviour rather than a row of numbers - which is
	/// the same line the wondrous table draws.
	/// </para>
	/// </summary>
	public abstract class DnDNamedWeapon : DnDWeapon, IDnDMagicItem
	{
		public virtual bool RequiresAttunement { get { return true; } }

		public virtual int AttackBonus { get { return 0; } }
		public virtual int DamageBonus { get { return 0; } }
		public virtual int ArmorClassBonus { get { return 0; } }
		public virtual int SavingThrowBonus { get { return 0; } }

		public virtual int GetAbilityScoreOverride(AbilityScoreType type) { return 0; }

		protected DnDNamedWeapon(string weaponId, string name, int hue)
			: base(weaponId)
		{
			Name = name;
			Hue = hue;
		}

		protected DnDNamedWeapon(Serial serial)
			: base(serial)
		{
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			base.AddNameProperty(list);

			if (AttackBonus != 0 || DamageBonus != 0)
			{
				list.Add(1049644, String.Format("+{0} attack, +{1} damage", AttackBonus, DamageBonus));
			}

			if (RequiresAttunement)
			{
				list.Add(1049644, "Requires Attunement");
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

	public class DnDFlameTongue : DnDNamedWeapon
	{
		public override string WeaponId { get { return "Longsword"; } }

		public override int DamageBonus { get { return 7; } } // 2d6 fire, averaged

		[Constructable]
		public DnDFlameTongue() : base("Longsword", "a Flame Tongue", 1354) { }

		public DnDFlameTongue(Serial serial) : base(serial) { }
	}

	public class DnDVorpalSword : DnDNamedWeapon
	{
		public override string WeaponId { get { return "Greatsword"; } }

		public override int AttackBonus { get { return 3; } }
		public override int DamageBonus { get { return 3; } }

		[Constructable]
		public DnDVorpalSword() : base("Greatsword", "a Vorpal Sword", 1150) { }

		public DnDVorpalSword(Serial serial) : base(serial) { }
	}

	public class DnDSunBlade : DnDNamedWeapon
	{
		public override string WeaponId { get { return "Longsword"; } }

		public override int AttackBonus { get { return 2; } }
		public override int DamageBonus { get { return 2; } }

		[Constructable]
		public DnDSunBlade() : base("Longsword", "a Sun Blade", 1153) { }

		public DnDSunBlade(Serial serial) : base(serial) { }
	}

	public class DnDHolyAvenger : DnDNamedWeapon
	{
		public override string WeaponId { get { return "Longsword"; } }

		public override int AttackBonus { get { return 3; } }
		public override int DamageBonus { get { return 3; } }

		[Constructable]
		public DnDHolyAvenger() : base("Longsword", "a Holy Avenger", 1150) { }

		public DnDHolyAvenger(Serial serial) : base(serial) { }
	}

	public class DnDMaceOfDisruption : DnDNamedWeapon
	{
		public override string WeaponId { get { return "Mace"; } }

		public override int AttackBonus { get { return 1; } }
		public override int DamageBonus { get { return 2; } }

		[Constructable]
		public DnDMaceOfDisruption() : base("Mace", "a Mace of Disruption", 1153) { }

		public DnDMaceOfDisruption(Serial serial) : base(serial) { }
	}

	public class DnDStaffOfTheMagi : DnDNamedWeapon
	{
		public override string WeaponId { get { return "Quarterstaff"; } }

		public override int AttackBonus { get { return 2; } }
		public override int DamageBonus { get { return 2; } }
		public override int SavingThrowBonus { get { return 2; } }

		[Constructable]
		public DnDStaffOfTheMagi() : base("Quarterstaff", "a Staff of the Magi", 1153) { }

		public DnDStaffOfTheMagi(Serial serial) : base(serial) { }
	}

	public class DnDStaffOfPower : DnDNamedWeapon
	{
		public override string WeaponId { get { return "Quarterstaff"; } }

		public override int AttackBonus { get { return 2; } }
		public override int DamageBonus { get { return 2; } }
		public override int ArmorClassBonus { get { return 2; } }
		public override int SavingThrowBonus { get { return 2; } }

		[Constructable]
		public DnDStaffOfPower() : base("Quarterstaff", "a Staff of Power", 1175) { }

		public DnDStaffOfPower(Serial serial) : base(serial) { }
	}

	public class DnDRodOfLordlyMight : DnDNamedWeapon
	{
		public override string WeaponId { get { return "Mace"; } }

		public override int AttackBonus { get { return 3; } }
		public override int DamageBonus { get { return 3; } }

		[Constructable]
		public DnDRodOfLordlyMight() : base("Mace", "a Rod of Lordly Might", 1153) { }

		public DnDRodOfLordlyMight(Serial serial) : base(serial) { }
	}

	/// <summary>
	/// A wand is a weapon you can also swing, which is why it is here rather than in the wondrous
	/// table - but what makes it a Wand of Magic Missiles is charges, and charges are the "items
	/// that do something on use" work still recorded in DND_TODO.md.
	/// </summary>
	public class DnDWandOfMagicMissiles : DnDNamedWeapon
	{
		public override string WeaponId { get { return "Club"; } }

		public override bool RequiresAttunement { get { return false; } }

		[Constructable]
		public DnDWandOfMagicMissiles() : base("Club", "a Wand of Magic Missiles", 1150) { }

		public DnDWandOfMagicMissiles(Serial serial) : base(serial) { }
	}
}
