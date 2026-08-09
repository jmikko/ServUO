using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a baba yaga’s horsemen corpse")]
    public sealed class SrdBabaYagaSHorsemen : SrdMonster
    {
        [Constructable]
        public SrdBabaYagaSHorsemen() : base("BabaYagaSHorsemen") 
        {
        }

        public SrdBabaYagaSHorsemen(Serial serial) : base(serial) { }
    }
}
