using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Engines.VvV;

namespace Server.Engines.Points
{
    // The various UO "points/loyalty" reward economies (casino chips, dungeon crystals,
    // Vice vs Virtue, PvP arena ladder, clean-up-Britannia tokens, etc.) have no D&D
    // equivalent and their concrete data was removed with the rest of the legacy content.
    // This stub keeps the shared PointsSystem/PointsEntry/PointsType shape alive so the
    // handful of still-standing item/gump/dungeon files that reference it keep compiling;
    // every system here is permanently disabled and reports zero points.
    public enum PointsType
    {
        None,
        ViceVsVirtue,
        PVPArena,
        Blackthorn,
        CasinoData,
        CleanUpBritannia,
        DespiseCrystals,
        FellowshipData,
        Khaldun,
        QueensLoyalty,
        RisingTide,
        ShameCrystals,
        VoidPool
    }

    public class PointsEntry
    {
        public PlayerMobile Player { get; set; }
        public double Points { get; set; }

        public PointsEntry(PlayerMobile pm)
        {
            Player = pm;
        }

        public virtual void Serialize(GenericWriter writer)
        {
            writer.Write(Points);
        }

        public virtual void Deserialize(GenericReader reader)
        {
            Points = reader.ReadDouble();
        }
    }

    public class PointsSystem
    {
        public static List<PointsSystem> Systems = new List<PointsSystem>();

        public virtual TextDefinition Name { get { return TextDefinition.Empty; } }
        public virtual bool ShowOnLoyaltyGump { get { return false; } }
        public virtual bool AutoAdd { get { return false; } }
        public virtual double MaxPoints { get { return 0; } }
        public virtual PointsType Loyalty { get { return PointsType.None; } }

        public virtual double GetPoints(Mobile m) { return 0; }
        public virtual void AwardPoints(Mobile m, double points, bool message = true) { }
        public virtual void DeductPoints(Mobile m, double points, bool message = true) { }
        public virtual TextDefinition GetTitle(PlayerMobile pm) { return null; }
        public virtual PointsEntry GetSystemEntry(PlayerMobile pm) { return null; }

        public PointsEntry GetEntry(Mobile m, bool create = false)
        {
            PlayerMobile pm = m as PlayerMobile;

            if (pm == null)
            {
                return null;
            }

            PointsEntry entry;

            if (!m_Entries.TryGetValue(pm, out entry) && create)
            {
                entry = GetSystemEntry(pm);

                if (entry != null)
                {
                    m_Entries[pm] = entry;
                }
            }

            return entry;
        }

        public T GetPlayerEntry<T>(Mobile m, bool create = false) where T : PointsEntry
        {
            return GetEntry(m, create) as T;
        }

        private readonly Dictionary<PlayerMobile, PointsEntry> m_Entries = new Dictionary<PlayerMobile, PointsEntry>();
        public virtual void SendMessage(PlayerMobile from, double old, double points, bool quest) { }
        public virtual void Serialize(GenericWriter writer) { }
        public virtual void Deserialize(GenericReader reader) { }

        public static PointsSystem GetSystemInstance(PointsType type)
        {
            return null;
        }

        public static readonly PointsSystem Blackthorn = new PointsSystem();
        public static readonly PointsSystem CasinoData = new PointsSystem();
        public static readonly CleanUpBritanniaData CleanUpBritannia = new CleanUpBritanniaData();
        public static readonly DespiseCrystalsPoints DespiseCrystals = new DespiseCrystalsPoints();
        public static readonly FellowshipPoints FellowshipData = new FellowshipPoints();
        public static readonly KhaldunPoints Khaldun = new KhaldunPoints();
        public static readonly QueensLoyaltyPoints QueensLoyalty = new QueensLoyaltyPoints();
        public static readonly RisingTidePoints RisingTide = new RisingTidePoints();
        public static readonly PointsSystem ShameCrystals = new PointsSystem();
        public static readonly PointsSystem VoidPool = new PointsSystem();
        public static readonly ViceVsVirtueSystem ViceVsVirtue = new ViceVsVirtueSystem();
    }

    public class CleanUpBritanniaData : PointsSystem
    {
        public static bool Enabled { get { return false; } }
        public static double GetPoints(Item item) { return 0; }

        public double GetPointsFromExchange(Mobile m) { return 0; }
        public void AddPointsToExchange(Mobile m) { }
        public void RemovePointsFromExchange(Mobile m) { }
    }

    public class DespiseCrystalsPoints : PointsSystem
    {
        public void ConvertFromOldSystem(Mobile m, int points) { }
    }

    public class FellowshipPoints : PointsSystem
    {
        public static bool Enabled { get { return false; } }
    }

    public class KhaldunPoints : PointsSystem
    {
        public bool InSeason { get { return false; } }
    }

    public class QueensLoyaltyPoints : PointsSystem
    {
        public bool IsNoble(Mobile m) { return false; }
        public static void ConvertFromOldSystem(Mobile m, int points) { }
    }

    public class RisingTidePoints : PointsSystem
    {
        public static bool Enabled { get { return false; } }
    }
}
