using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a coven green hag corpse")]
    public sealed class SrdCovenGreenHag : SrdMonster
    {
        [Constructable]
        public SrdCovenGreenHag() : base("CovenGreenHag") 
        {
        }

        public SrdCovenGreenHag(Serial serial) : base(serial) { }
    }
}
