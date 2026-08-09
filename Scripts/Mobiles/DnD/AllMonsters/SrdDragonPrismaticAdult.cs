using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragon, prismatic adult corpse")]
    public sealed class SrdDragonPrismaticAdult : SrdMonster
    {
        [Constructable]
        public SrdDragonPrismaticAdult() : base("DragonPrismaticAdult") 
        {
        }

        public SrdDragonPrismaticAdult(Serial serial) : base(serial) { }
    }
}
