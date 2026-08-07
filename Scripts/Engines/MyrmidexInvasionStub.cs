namespace Server.Engines.MyrmidexInvasion
{
    // The Myrmidex Invasion world event (Eodon tribes vs. Myrmidex hive war) has no D&D
    // equivalent and its spawner/region logic was removed with the rest of the legacy
    // content. This stub keeps the small set of types the surviving Time of Legends
    // monster mobiles reference so they keep compiling; the event is permanently inactive.
    public static class MyrmidexInvasionSystem
    {
        public static bool Active { get { return false; } }

        public static bool IsAlliedWithEodonTribes(Mobile m) { return false; }
        public static bool IsAlliedWithMyrmidex(Mobile m) { return false; }
        public static bool AreEnemies(Mobile m1, Mobile m2) { return false; }
    }

    public class BattleRegion : Region
    {
        public BattleRegion(string name, Map map, int priority) : base(name, map, priority)
        {
        }
    }
}
