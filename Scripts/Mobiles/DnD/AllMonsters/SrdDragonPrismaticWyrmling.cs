using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragon, prismatic wyrmling corpse")]
    public sealed class SrdDragonPrismaticWyrmling : SrdMonster
    {
        [Constructable]
        public SrdDragonPrismaticWyrmling() : base("DragonPrismaticWyrmling") 
        {
        }

        public SrdDragonPrismaticWyrmling(Serial serial) : base(serial) { }
    }
}
