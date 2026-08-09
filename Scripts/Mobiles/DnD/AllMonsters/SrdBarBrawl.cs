using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bar brawl corpse")]
    public sealed class SrdBarBrawl : SrdMonster
    {
        [Constructable]
        public SrdBarBrawl() : base("BarBrawl") 
        {
        }

        public SrdBarBrawl(Serial serial) : base(serial) { }
    }
}
