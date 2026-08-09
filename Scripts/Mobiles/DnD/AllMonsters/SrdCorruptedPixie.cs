using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corrupted pixie corpse")]
    public sealed class SrdCorruptedPixie : SrdMonster
    {
        [Constructable]
        public SrdCorruptedPixie() : base("CorruptedPixie") 
        {
        }

        public SrdCorruptedPixie(Serial serial) : base(serial) { }
    }
}
