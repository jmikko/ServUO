using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dire wildebeest corpse")]
    public sealed class SrdDireWildebeest : SrdMonster
    {
        [Constructable]
        public SrdDireWildebeest() : base("DireWildebeest") 
        {
        }

        public SrdDireWildebeest(Serial serial) : base(serial) { }
    }
}
