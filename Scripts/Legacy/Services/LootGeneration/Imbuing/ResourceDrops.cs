using Server;
using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Items
{
    public class IngredientDropEntry
    {
        private Type m_CreatureType;
        private bool m_DropMultiples;
        private string m_Region;
        private double m_Chance;
        private Type[] m_Ingredients;

        public Type CreatureType { get { return m_CreatureType; } }
        public bool DropMultiples { get { return m_DropMultiples; } }
        public string Region { get { return m_Region; } }
        public double Chance { get { return m_Chance; } }
        public Type[] Ingredients { get { return m_Ingredients; } }

        public IngredientDropEntry(Type creature, bool dropMultiples, double chance, params Type[] ingredients)
            : this(creature, dropMultiples, null, chance, ingredients)
        {
        }

        public IngredientDropEntry(Type creature, bool dropMultiples, string region, double chance, params Type[] ingredients)
        {
            m_CreatureType = creature;
            m_Ingredients = ingredients;
            m_DropMultiples = dropMultiples;
            m_Region = region;
            m_Chance = chance;
        }

        private static List<IngredientDropEntry> m_IngredientTable;
        public static List<IngredientDropEntry> IngredientTable { get { return m_IngredientTable; } }

        public static void Initialize()
        {
            EventSink.CreatureDeath += OnCreatureDeath;

            m_IngredientTable = new List<IngredientDropEntry>();

            // Imbuing Gems
        }

        public static void OnCreatureDeath(CreatureDeathEventArgs e)
        {
            BaseCreature bc = e.Creature as BaseCreature;
            Container c = e.Corpse;

            if (bc != null && c != null && !c.Deleted && !bc.Controlled && !bc.Summoned)
            {
                CheckDrop(bc, c);
            }

            if (e.Killer is BaseVoidCreature)
            {
                ((BaseVoidCreature)e.Killer).Mutate(VoidEvolution.Killing);
            }
        }

        public static void CheckDrop(BaseCreature bc, Container c)
        {
            if (m_IngredientTable != null)
            {
                foreach (IngredientDropEntry entry in m_IngredientTable)
                {
                    if (entry == null)
                        continue;

                    if (entry.Region != null)
                    {
                        string reg = entry.Region;

                        if (reg == "TerMur" && c.Map != Map.TerMur)
                        {
                            continue;
                        }
                        else if (reg == "Abyss" && (c.Map != Map.TerMur || c.X < 235 || c.X > 1155 || c.Y < 40 || c.Y > 1040))
                        {
                            continue;
                        }
                        else if (reg != "TerMur" && reg != "Abyss")
                        {
                            Server.Region r = Server.Region.Find(c.Location, c.Map);

                            if (r == null || !r.IsPartOf(entry.Region))
                                continue;
                        }
                    }

                    if (bc.GetType() != entry.CreatureType && !bc.GetType().IsSubclassOf(entry.CreatureType))
                    {
                        continue;
                    }

                    double toBeat = entry.Chance;
                    List<Item> drops = new List<Item>();

                    if (bc is BaseVoidCreature)
                    {
                        toBeat *= ((BaseVoidCreature)bc).Stage + 1;
                    }

                    if (entry.DropMultiples)
                    {
                        foreach (Type type in entry.Ingredients)
                        {
                            if (toBeat >= Utility.RandomDouble())
                            {
                                Item drop = Loot.Construct(type);

                                if (drop != null)
                                    drops.Add(drop);
                            }
                        }
                    }
                    else if (toBeat >= Utility.RandomDouble())
                    {
                        Item drop = Loot.Construct(entry.Ingredients);

                        if (drop != null)
                            drops.Add(drop);
                    }

                    foreach (Item item in drops)
                    {
                        c.DropItem(item);
                    }

                    ColUtility.Free(drops);
                }
            }
        }

        public static Type[] ImbuingGems = 
        {
            typeof(FireRuby), 
            typeof(WhitePearl), 
            typeof(BlueDiamond), 
			typeof(Turquoise)
        };
    }
}
