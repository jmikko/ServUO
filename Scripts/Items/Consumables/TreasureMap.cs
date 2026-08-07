using System;
using System.Collections.Generic;

namespace Server.Items
{
    // The treasure-hunting system (decoded maps, dig sites, guardian spawns, chest loot tiers) was
    // legacy UO content with no D&D equivalent and its implementation was removed. These types are
    // kept as inert stubs because a treasure map is still a craftable/lootable *item* referenced by
    // cartography, fishing, paragon chests, house storage and the lockpick/unlock code paths - the
    // item exists and can be held, it simply never decodes into a dig site.
    public enum TreasureLevel
    {
        Stash,
        Supply,
        Cache,
        Hoard,
        Trove
    }

    public static class TreasureMapInfo
    {
        public static bool NewSystem { get { return false; } }

        public static int ConvertLevel(int level)
        {
            return level;
        }
    }

    public class TreasureMap : MapItem
    {
        private int m_Level;
        private Map m_Facet;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level { get { return m_Level; } set { m_Level = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map Facet { get { return m_Facet; } set { m_Facet = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Completed { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile CompletedBy { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Decoder { get; set; }

        [Constructable]
        public TreasureMap(int level, Map facet)
        {
            m_Level = level;
            m_Facet = facet;
        }

        public TreasureMap(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1015230; } } // a treasure map

        public static bool IsInHavenIsland(IPoint2D loc)
        {
            return false;
        }

        public static bool ValidateLocation(int x, int y, Map map)
        {
            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_Level);
            writer.Write(m_Facet);
            writer.Write(Completed);
            writer.Write(CompletedBy);
            writer.Write(Decoder);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt(); // version

            m_Level = reader.ReadInt();
            m_Facet = reader.ReadMap();
            Completed = reader.ReadBool();
            CompletedBy = reader.ReadMobile();
            Decoder = reader.ReadMobile();
        }
    }

    public class TreasureMapChest : LockableContainer
    {
        public List<Mobile> Guardians { get; private set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get; set; }

        [Constructable]
        public TreasureMapChest(int level)
            : base(0xE40)
        {
            Level = level;
            Guardians = new List<Mobile>();
            Movable = false;
        }

        public TreasureMapChest(Serial serial)
            : base(serial)
        {
            Guardians = new List<Mobile>();
        }

        public static void GetRandomItemStat(out int min, out int max)
        {
            min = 1;
            max = 1;
        }

        public static void Fill(Mobile from, LockableContainer cont, int level, bool isSos)
        {
            // Treasure chest loot generation removed with the treasure-hunting system.
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(Level);
            writer.Write(Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt(); // version

            Level = reader.ReadInt();
            Owner = reader.ReadMobile();
        }
    }
}
