using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bull corpse")]
    public sealed class SrdBull : SrdMonster
    {
        [Constructable]
        public SrdBull() : base("Bull") 
        {
        }

        public SrdBull(Serial serial) : base(serial) { }
    }
}
