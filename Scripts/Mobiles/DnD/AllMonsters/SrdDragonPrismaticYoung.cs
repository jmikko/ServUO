using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragon, prismatic young corpse")]
    public sealed class SrdDragonPrismaticYoung : SrdMonster
    {
        [Constructable]
        public SrdDragonPrismaticYoung() : base("DragonPrismaticYoung") 
        {
        }

        public SrdDragonPrismaticYoung(Serial serial) : base(serial) { }
    }
}
