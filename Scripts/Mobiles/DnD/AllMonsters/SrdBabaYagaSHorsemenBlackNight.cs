using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a baba yaga's horsemen, black night corpse")]
    public sealed class SrdBabaYagaSHorsemenBlackNight : SrdMonster
    {
        [Constructable]
        public SrdBabaYagaSHorsemenBlackNight() : base("BabaYagaSHorsemenBlackNight") 
        {
        }

        public SrdBabaYagaSHorsemenBlackNight(Serial serial) : base(serial) { }
    }
}
