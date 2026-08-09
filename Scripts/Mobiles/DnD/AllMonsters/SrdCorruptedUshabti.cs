using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corrupted ushabti corpse")]
    public sealed class SrdCorruptedUshabti : SrdMonster
    {
        [Constructable]
        public SrdCorruptedUshabti() : base("CorruptedUshabti") 
        {
        }

        public SrdCorruptedUshabti(Serial serial) : base(serial) { }
    }
}
