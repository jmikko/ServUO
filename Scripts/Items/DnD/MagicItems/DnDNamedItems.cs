using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DnDAdamantineArmor : DnDMagicArmor
    {
        public override string ArmorId { get { return "HalfPlate"; } }
        
        [Constructable]
        public DnDAdamantineArmor() : base("HalfPlate")
        {
            Name = "Adamantine Armor (Half Plate)";
            ItemID = 0x1412; // Half plate
            Hue = 1150;
        }

        public DnDAdamantineArmor(Serial serial) : base(serial) { }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add(1070722, "Any critical hit against you becomes a normal hit.");
        }

        public override void OnTakeDamage(Mobile attacker, Mobile defender, ref int damage, bool critical)
        {
            if (critical)
            {
                defender.SendMessage("Your Adamantine Armor hardens, turning a critical blow into a normal hit!");
            }
            base.OnTakeDamage(attacker, defender, ref damage, critical);
        }

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

    public class DnDAnimatedShield : DnDMagicArmor
    {
        public override string ArmorId { get { return "Shield"; } }
        public override bool RequiresAttunement { get { return true; } }
        
        [Constructable]
        public DnDAnimatedShield() : base("Shield")
        {
            Name = "Animated Shield";
            ItemID = 0x1B76; // Shield
            Hue = 1150;
        }

        public DnDAnimatedShield(Serial serial) : base(serial) { }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add(1070722, "You can speak a command word to cause the shield to animate and hover, freeing your hands.");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack) || Parent == from)
            {
                from.SendMessage("You speak the command word, and the shield leaps into the air to protect you, freeing up your hand!");
                from.PlaySound(0x1F4);
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }

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

    public class DnDDaggerOfVenom : DnDMagicWeapon
    {
        public override string WeaponId { get { return "Dagger"; } }
        public override int AttackBonus { get { return 1; } }
        public override int DamageBonus { get { return 1; } }
        
        private bool m_PoisonActive;

        [Constructable]
        public DnDDaggerOfVenom() : base("Dagger")
        {
            Name = "Dagger of Venom";
            ItemID = 0xF52; // Dagger
            Hue = 0x8A4; // Poison hue
        }

        public DnDDaggerOfVenom(Serial serial) : base(serial) { }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add(1070722, "+1 Dagger. Use to coat the blade in thick poison for your next hit.");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack) || Parent == from)
            {
                if (!m_PoisonActive)
                {
                    m_PoisonActive = true;
                    from.SendMessage("Thick, black poison coats the blade of the dagger.");
                    from.PlaySound(0x236);
                }
                else
                {
                    from.SendMessage("The blade is already coated in poison.");
                }
            }
        }

        public override void OnHit(Mobile attacker, IDamageable defender, int damage, bool critical)
        {
            if (m_PoisonActive)
            {
                attacker.SendMessage("Your Dagger of Venom strikes true, delivering its deadly payload!");
                if (defender is Mobile)
                    ((Mobile)defender).SendMessage("You are injected with virulent poison!");
                defender.Damage(2, attacker);
                m_PoisonActive = false;
            }
            base.OnHit(attacker, defender, damage, critical);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_PoisonActive);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_PoisonActive = reader.ReadBool();
        }
    }

    public class DnDSwordOfLifeStealing : DnDMagicWeapon
    {
        public override string WeaponId { get { return "Longsword"; } }
        public override bool RequiresAttunement { get { return true; } }

        [Constructable]
        public DnDSwordOfLifeStealing() : base("Longsword")
        {
            Name = "Sword of Life Stealing";
            ItemID = 0xF61; // Longsword
            Hue = 0x481;
        }

        public DnDSwordOfLifeStealing(Serial serial) : base(serial) { }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add(1070722, "On a critical hit, deals extra necrotic damage and grants you temporary hit points.");
        }

        public override void OnHit(Mobile attacker, IDamageable defender, int damage, bool critical)
        {
            if (critical)
            {
                attacker.SendMessage("The sword drains the life force of your enemy, revitalizing you!");
                defender.Damage(10, attacker);
                attacker.Hits = Math.Min(attacker.HitsMax, attacker.Hits + 10);
                attacker.FixedParticles(0x375A, 1, 15, 5005, 1153, 0, EffectLayer.Waist);
            }
            base.OnHit(attacker, defender, damage, critical);
        }

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

    public class DnDPeriaptOfProofAgainstPoison : DnDWondrousItem
    {
        public override string WondrousId { get { return "PeriaptOfProofAgainstPoison"; } }

        [Constructable]
        public DnDPeriaptOfProofAgainstPoison() : base("PeriaptOfProofAgainstPoison")
        {
            Name = "Periapt of Proof against Poison";
            ItemID = 0x1087; // Amulet
            Hue = 0x1A6;
        }

        public DnDPeriaptOfProofAgainstPoison(Serial serial) : base(serial) { }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add(1070722, "You are immune to poison damage and the poisoned condition.");
        }

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

    public class DnDMaceOfTerror : DnDMagicWeapon
    {
        public override string WeaponId { get { return "Mace"; } }
        public override bool RequiresAttunement { get { return true; } }

        [Constructable]
        public DnDMaceOfTerror() : base("Mace")
        {
            Name = "Mace of Terror";
            ItemID = 0xF5C; // Mace
            Hue = 0x497;
        }

        public DnDMaceOfTerror(Serial serial) : base(serial) { }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add(1070722, "Use to unleash a terrifying wave of magic.");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack) || Parent == from)
            {
                from.SendMessage("You unleash the magic of the Mace of Terror, causing dread in your enemies!");
                from.PlaySound(0x110);
                from.FixedParticles(0x3709, 1, 30, 9904, 1108, 6, EffectLayer.Waist);
            }
        }

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

    public class DnDDragonSlayer : DnDMagicWeapon
    {
        public override string WeaponId { get { return "Longsword"; } }

        [Constructable]
        public DnDDragonSlayer() : base("Longsword")
        {
            Name = "Dragon Slayer";
            ItemID = 0xF61; // Longsword
            Hue = 0x8A5;
        }

        public DnDDragonSlayer(Serial serial) : base(serial) { }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add(1070722, "Deals extra damage to dragons.");
        }

        public override void OnHit(Mobile attacker, IDamageable defender, int damage, bool critical)
        {
            if (defender is Mobile)
            {
                if (defender.GetType().Name.Contains("Dragon") || defender.GetType().Name.Contains("Drake"))
                {
                    attacker.SendMessage("Your Dragon Slayer sword bites deeply into the draconic flesh!");
                    defender.Damage(10, attacker);
                }
            }
            base.OnHit(attacker, defender, damage, critical);
        }

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

    public class DnDStaffOfFrost : DnDMagicWeapon
    {
        public override string WeaponId { get { return "Quarterstaff"; } }
        public override bool RequiresAttunement { get { return true; } }

        [Constructable]
        public DnDStaffOfFrost() : base("Quarterstaff")
        {
            Name = "Staff of Frost";
            ItemID = 0xE89; // Quarterstaff
            Hue = 0x47F;
        }

        public DnDStaffOfFrost(Serial serial) : base(serial) { }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add(1070722, "Can be used to cast cold-based spells. Deals extra cold damage on a critical hit.");
        }

        public override void OnHit(Mobile attacker, IDamageable defender, int damage, bool critical)
        {
            if (critical)
            {
                attacker.SendMessage("The staff flares with freezing energy!");
                defender.Damage(5, attacker);
            }
            base.OnHit(attacker, defender, damage, critical);
        }

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

    public class DnDMantleOfSpellResistance : DnDWondrousItem
    {
        public override string WondrousId { get { return "MantleOfSpellResistance"; } }

        [Constructable]
        public DnDMantleOfSpellResistance() : base("MantleOfSpellResistance")
        {
            Name = "Mantle of Spell Resistance";
            ItemID = 0x1515; // Cloak
            Hue = 0x183;
        }

        public DnDMantleOfSpellResistance(Serial serial) : base(serial) { }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add(1070722, "You have advantage on saving throws against spells.");
        }

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

    public class DnDDawnbringer : DnDMagicWeapon
    {
        public override string WeaponId { get { return "Longsword"; } }
        public override bool RequiresAttunement { get { return true; } }

        [Constructable]
        public DnDDawnbringer() : base("Longsword")
        {
            Name = "Dawnbringer";
            ItemID = 0xF61; // Longsword
            Hue = 0x481; // glowing hue
        }

        public DnDDawnbringer(Serial serial) : base(serial) { }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add(1070722, "A sentient sun blade. Emits bright sunlight. Deals extra radiant damage to undead.");
        }

        public override void OnHit(Mobile attacker, IDamageable defender, int damage, bool critical)
        {
            if (defender is Mobile)
            {
                if (defender.GetType().Name.Contains("Skeleton") || defender.GetType().Name.Contains("Zombie") || defender.GetType().Name.Contains("Vampire") || defender.GetType().Name.Contains("Lich"))
                {
                    attacker.SendMessage("Dawnbringer burns the undead flesh with radiant sunlight!");
                    defender.Damage(5, attacker);
                }
            }
            base.OnHit(attacker, defender, damage, critical);
        }

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
