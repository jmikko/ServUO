using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a carrion beetle corpse")]
    public sealed class SrdCarrionBeetle : SrdMonster
    {
        [Constructable]
        public SrdCarrionBeetle() : base("CarrionBeetle") 
        {
        }

        public SrdCarrionBeetle(Serial serial) : base(serial) { }
    }
}
