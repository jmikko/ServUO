using System;
using Server.Mobiles;

namespace Server.Items
{
    /// <summary>
    /// The "slayer weapon" bonus-damage-vs-monster-type system has no D&amp;D equivalent and its data
    /// was almost entirely typeof() references into the legacy monster roster (~940 files being
    /// removed). Rather than deleting this class outright - other item code (weapon/talisman slayer
    /// properties) still references the SlayerName enum and SlayerGroup/SlayerEntry shapes - every
    /// group here keeps its original SlayerName identity (so that indexing and any code doing
    /// "does this weapon have SlayerName.X" keeps compiling and behaving sanely), but every FoundOn/
    /// Entries/Super monster-type list is now empty. The mechanic is inert: Slays() always returns
    /// false, so slayer weapons simply never trigger their bonus. This is intentional, not a bug.
    /// </summary>
    public class SlayerGroup
    {
        private static SlayerEntry[] m_TotalEntries;
        private static SlayerGroup[] m_Groups;
        private SlayerGroup[] m_Opposition;
        private SlayerEntry m_Super;
        private SlayerEntry[] m_Entries;
        private Type[] m_FoundOn;

        public SlayerGroup()
        {
        }

        static SlayerGroup()
        {
            SlayerGroup humanoid = new SlayerGroup();
            SlayerGroup undead = new SlayerGroup();
            SlayerGroup elemental = new SlayerGroup();
            SlayerGroup abyss = new SlayerGroup();
            SlayerGroup arachnid = new SlayerGroup();
            SlayerGroup reptilian = new SlayerGroup();
            SlayerGroup fey = new SlayerGroup();
            SlayerGroup eodon = new SlayerGroup();
            SlayerGroup eodonTribe = new SlayerGroup();
            SlayerGroup dino = new SlayerGroup();
            SlayerGroup myrmidex = new SlayerGroup();

            humanoid.Opposition = new SlayerGroup[] { undead };
            humanoid.FoundOn = new Type[] { };
            humanoid.Super = new SlayerEntry(SlayerName.Repond);
            humanoid.Entries = new SlayerEntry[]
            {
                new SlayerEntry(SlayerName.OgreTrashing),
                new SlayerEntry(SlayerName.OrcSlaying),
                new SlayerEntry(SlayerName.TrollSlaughter),
            };

            undead.Opposition = new SlayerGroup[] { humanoid };
            undead.FoundOn = new Type[] { };
            undead.Super = new SlayerEntry(SlayerName.Silver);
            undead.Entries = new SlayerEntry[0];

            fey.Opposition = new SlayerGroup[] { abyss };
            fey.FoundOn = new Type[] { };
            fey.Super = new SlayerEntry(SlayerName.Fey);
            fey.Entries = new SlayerEntry[0];

            elemental.Opposition = new SlayerGroup[] { abyss };
            elemental.FoundOn = new Type[] { };
            elemental.Super = new SlayerEntry(SlayerName.ElementalBan);
            elemental.Entries = new SlayerEntry[]
            {
                new SlayerEntry(SlayerName.BloodDrinking),
                new SlayerEntry(SlayerName.EarthShatter),
                new SlayerEntry(SlayerName.ElementalHealth),
                new SlayerEntry(SlayerName.FlameDousing),
                new SlayerEntry(SlayerName.SummerWind),
                new SlayerEntry(SlayerName.Vacuum),
                new SlayerEntry(SlayerName.WaterDissipation),
            };

            abyss.Opposition = new SlayerGroup[] { elemental, fey };
            abyss.FoundOn = new Type[] { };

            if (Core.AOS)
            {
                abyss.Super = new SlayerEntry(SlayerName.Exorcism);
                abyss.Entries = new SlayerEntry[]
                {
                    // Daemon Dismissal & Balron Damnation have been removed and moved up to super slayer on OSI.
                    new SlayerEntry(SlayerName.GargoylesFoe),
                };
            }
            else
            {
                abyss.Super = new SlayerEntry(SlayerName.Exorcism);
                abyss.Entries = new SlayerEntry[]
                {
                    new SlayerEntry(SlayerName.DaemonDismissal),
                    new SlayerEntry(SlayerName.GargoylesFoe),
                    new SlayerEntry(SlayerName.BalronDamnation),
                };
            }

            arachnid.Opposition = new SlayerGroup[] { reptilian };
            arachnid.FoundOn = new Type[] { };
            arachnid.Super = new SlayerEntry(SlayerName.ArachnidDoom);
            arachnid.Entries = new SlayerEntry[]
            {
                new SlayerEntry(SlayerName.ScorpionsBane),
                new SlayerEntry(SlayerName.SpidersDeath),
                new SlayerEntry(SlayerName.Terathan),
            };

            reptilian.Opposition = new SlayerGroup[] { arachnid };
            reptilian.FoundOn = new Type[] { };
            reptilian.Super = new SlayerEntry(SlayerName.ReptilianDeath);
            reptilian.Entries = new SlayerEntry[]
            {
                new SlayerEntry(SlayerName.DragonSlaying),
                new SlayerEntry(SlayerName.LizardmanSlaughter),
                new SlayerEntry(SlayerName.Ophidian),
                new SlayerEntry(SlayerName.SnakesBane),
            };

            eodon.Opposition = new SlayerGroup[] { };
            eodon.FoundOn = new Type[] { };
            eodon.Super = new SlayerEntry(SlayerName.Eodon);
            eodon.Entries = new SlayerEntry[] { };

            eodonTribe.Opposition = new SlayerGroup[] { };
            eodonTribe.FoundOn = new Type[] { };
            eodonTribe.Super = new SlayerEntry(SlayerName.EodonTribe);
            eodonTribe.Entries = new SlayerEntry[] { };

            dino.Opposition = new SlayerGroup[] { fey };
            dino.FoundOn = new Type[] { };
            dino.Super = new SlayerEntry(SlayerName.Dinosaur);
            dino.Entries = new SlayerEntry[] { };

            myrmidex.Opposition = new SlayerGroup[] { fey };
            myrmidex.FoundOn = new Type[] { };
            myrmidex.Super = new SlayerEntry(SlayerName.Myrmidex);
            myrmidex.Entries = new SlayerEntry[] { };

            m_Groups = new SlayerGroup[]
            {
                humanoid,
                undead,
                elemental,
                abyss,
                arachnid,
                reptilian,
                fey,
                eodon,
                eodonTribe,
                dino,
                myrmidex,
            };

            m_TotalEntries = CompileEntries(m_Groups);
        }

        public static SlayerEntry[] TotalEntries
        {
            get
            {
                return m_TotalEntries;
            }
        }
        public static SlayerGroup[] Groups
        {
            get
            {
                return m_Groups;
            }
        }
        public SlayerGroup[] Opposition
        {
            get
            {
                return this.m_Opposition;
            }
            set
            {
                this.m_Opposition = value;
            }
        }
        public SlayerEntry Super
        {
            get
            {
                return this.m_Super;
            }
            set
            {
                this.m_Super = value;
            }
        }
        public SlayerEntry[] Entries
        {
            get
            {
                return this.m_Entries;
            }
            set
            {
                this.m_Entries = value;
            }
        }
        public Type[] FoundOn
        {
            get
            {
                return this.m_FoundOn;
            }
            set
            {
                this.m_FoundOn = value;
            }
        }
        public static SlayerEntry GetEntryByName(SlayerName name)
        {
            int v = (int)name;

            if (v >= 0 && v < m_TotalEntries.Length)
                return m_TotalEntries[v];

            return null;
        }

        public static SlayerName GetLootSlayerType(Type type)
        {
            for (int i = 0; i < m_Groups.Length; ++i)
            {
                SlayerGroup group = m_Groups[i];
                Type[] foundOn = group.FoundOn;

                bool inGroup = false;

                for (int j = 0; foundOn != null && !inGroup && j < foundOn.Length; ++j)
                    inGroup = (foundOn[j] == type);

                if (inGroup)
                {
                    int index = Utility.Random(1 + group.Entries.Length);

                    if (index == 0)
                        return group.m_Super.Name;

                    return group.Entries[index - 1].Name;
                }
            }

            return SlayerName.Silver;
        }

        public bool OppositionSuperSlays(Mobile m)
        {
            for (int i = 0; i < this.Opposition.Length; i++)
            {
                if (this.Opposition[i].Super.Slays(m))
                    return true;
            }

            if (m_Super.Name == SlayerName.Eodon && !m_Super.Slays(m))
                return true;

            return false;
        }

        private static SlayerEntry[] CompileEntries(SlayerGroup[] groups)
        {
            SlayerEntry[] entries = new SlayerEntry[32];

            for (int i = 0; i < groups.Length; ++i)
            {
                SlayerGroup g = groups[i];

                g.Super.Group = g;

                entries[(int)g.Super.Name] = g.Super;

                for (int j = 0; j < g.Entries.Length; ++j)
                {
                    g.Entries[j].Group = g;
                    entries[(int)g.Entries[j].Name] = g.Entries[j];
                }
            }

            return entries;
        }

        public static SlayerName RandomSuperSlayerAOS(bool excludeFey = true)
        {
            int maxIndex = excludeFey ? 5 : 6;

            return Groups[Utility.Random(maxIndex)].Super.Name;
        }

        public static SlayerName RandomSuperSlayerTOL()
        {
            return Groups[Utility.Random(Groups.Length)].Super.Name;
        }
    }
}
