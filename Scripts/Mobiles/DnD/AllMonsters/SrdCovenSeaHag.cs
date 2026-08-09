using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a coven sea hag corpse")]
    public sealed class SrdCovenSeaHag : SrdMonster
    {
        [Constructable]
        public SrdCovenSeaHag() : base("CovenSeaHag") 
        {
        }

        public SrdCovenSeaHag(Serial serial) : base(serial) { }
    }
}
