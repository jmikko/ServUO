using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a baba yaga's horsemen, bright day corpse")]
    public sealed class SrdBabaYagaSHorsemenBrightDay : SrdMonster
    {
        [Constructable]
        public SrdBabaYagaSHorsemenBrightDay() : base("BabaYagaSHorsemenBrightDay") 
        {
        }

        public SrdBabaYagaSHorsemenBrightDay(Serial serial) : base(serial) { }
    }
}
