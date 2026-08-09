using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a coven winter hag corpse")]
    public sealed class SrdCovenWinterHag : SrdMonster
    {
        [Constructable]
        public SrdCovenWinterHag() : base("CovenWinterHag") 
        {
        }

        public SrdCovenWinterHag(Serial serial) : base(serial) { }
    }
}
