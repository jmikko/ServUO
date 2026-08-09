using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragon, prismatic ancient corpse")]
    public sealed class SrdDragonPrismaticAncient : SrdMonster
    {
        [Constructable]
        public SrdDragonPrismaticAncient() : base("DragonPrismaticAncient") 
        {
        }

        public SrdDragonPrismaticAncient(Serial serial) : base(serial) { }
    }
}
