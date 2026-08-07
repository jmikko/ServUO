using System;

namespace Server.Items
{
    // Base-tier Imbuing ingredients referenced throughout ItemPropertyInfo.cs's runic/reforging
    // tables but whose original definitions were lost; reconstructed here as plain stackable
    // resource items matching the shape of their sibling ingredients (Tourmaline, CrystalShards, etc).
    public class RelicFragment : Item
    {
        [Constructable]
        public RelicFragment()
            : this(1)
        {
        }

        [Constructable]
        public RelicFragment(int amount)
            : base(0x572C)
        {
            Stackable = true;
            Amount = amount;
        }

        public RelicFragment(Serial serial)
            : base(serial)
        {
        }

        public override double DefaultWeight { get { return 0.1; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class EnchantedEssence : Item
    {
        [Constructable]
        public EnchantedEssence()
            : this(1)
        {
        }

        [Constructable]
        public EnchantedEssence(int amount)
            : base(0x571B)
        {
            Stackable = true;
            Amount = amount;
        }

        public EnchantedEssence(Serial serial)
            : base(serial)
        {
        }

        public override double DefaultWeight { get { return 0.1; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class MagicalResidue : Item
    {
        [Constructable]
        public MagicalResidue()
            : this(1)
        {
        }

        [Constructable]
        public MagicalResidue(int amount)
            : base(0x5735)
        {
            Stackable = true;
            Amount = amount;
        }

        public MagicalResidue(Serial serial)
            : base(serial)
        {
        }

        public override double DefaultWeight { get { return 0.1; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
