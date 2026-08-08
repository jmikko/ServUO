using Server.Mobiles;

namespace Server.Items
{
    // Recall-target map items from two removed legacy UO player-economy systems: the vendor
    // search (find a player vendor selling X, then recall to it) and the auction house. The
    // systems are gone, but RecallSpell still has dedicated constructors and travel hooks for
    // them, so the item shapes are kept as inert stubs - neither can be obtained in-game.
    public class VendorSearchMap : Item
    {
        [Constructable]
        public VendorSearchMap() : base(0x14EC)
        {
        }

        public VendorSearchMap(Serial serial) : base(serial)
        {
        }

        public Point3D GetLocation(Mobile m) { return Point3D.Zero; }
        public Map GetMap() { return null; }
        public void OnBeforeTravel(Mobile m) { }

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

    public class AuctionMap : Item
    {
        [Constructable]
        public AuctionMap() : base(0x14EC)
        {
        }

        public AuctionMap(Serial serial) : base(serial)
        {
        }

        public Point3D GetLocation(Mobile m) { return Point3D.Zero; }
        public Map GetMap() { return null; }
        public void OnBeforeTravel(Mobile m) { }

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

    // Crafting talisman that could salvage resources on a failed craft. Its concrete item lived
    // with the removed talisman-reward content; the craft system still threads a reference
    // through ConsumeOnFailure, so the type is kept and simply never obtainable.
    public class MasterCraftsmanTalisman : BaseTalisman
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public int Type { get; set; }

        [Constructable]
        public MasterCraftsmanTalisman() : base(0x2F5B)
        {
        }

        public MasterCraftsmanTalisman(Serial serial) : base(serial)
        {
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
