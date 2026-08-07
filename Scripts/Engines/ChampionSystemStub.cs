namespace Server.Engines.CannedEvil
{
    // The champion spawn system (dungeon boss altars, skull rewards) has no D&D equivalent
    // and its concrete spawner/altar logic was removed with the rest of the legacy content.
    // This stub keeps the small set of types still-standing boss classes reference so they
    // keep compiling; ChampionSpawn is never actually placed by any spawner anymore.
    public enum ChampionSkullType
    {
        None
    }

    public enum ChampionSpawnType
    {
        Abyss, Arachnid, ColdBlood, ForestLord, SleepingDragon, UnholyTerror, VerminHorde, Glade, Corrupt, DragonTurtle
    }

    public class ChampionSpawn : Item
    {
        public ChampionSpawnType Type { get; set; }
        public double SpawnRadius { get; set; }
        public double SpawnMod { get; set; }
        public double KillsMod { get; set; }
        public bool Active { get; set; }

        public ChampionSpawn() : base(0x1F13)
        {
            Movable = false;
            Visible = false;
        }

        public ChampionSpawn(Serial serial) : base(serial)
        {
        }

        public bool IsChampionSpawn(Mobile m)
        {
            return false;
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

namespace Server.Items
{
    public class ChampionSkull : Item
    {
        public ChampionSkull(Server.Engines.CannedEvil.ChampionSkullType type) : base(0x3195)
        {
        }

        public ChampionSkull(Serial serial) : base(serial)
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
