using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a asanbosam corpse")]
    public sealed class SrdAsanbosam : SrdMonster
    {
        [Constructable]
        public SrdAsanbosam() : base("Asanbosam") 
        {
        }

        public SrdAsanbosam(Serial serial) : base(serial) { }
    }
}
