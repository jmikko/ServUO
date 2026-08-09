using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corrupted chieftain corpse")]
    public sealed class SrdCorruptedChieftain : SrdMonster
    {
        [Constructable]
        public SrdCorruptedChieftain() : base("CorruptedChieftain") 
        {
        }

        public SrdCorruptedChieftain(Serial serial) : base(serial) { }
    }
}
