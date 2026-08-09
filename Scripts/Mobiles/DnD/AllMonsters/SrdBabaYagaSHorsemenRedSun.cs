using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a baba yaga's horsemen, red sun corpse")]
    public sealed class SrdBabaYagaSHorsemenRedSun : SrdMonster
    {
        [Constructable]
        public SrdBabaYagaSHorsemenRedSun() : base("BabaYagaSHorsemenRedSun") 
        {
        }

        public SrdBabaYagaSHorsemenRedSun(Serial serial) : base(serial) { }
    }
}
