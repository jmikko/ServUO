using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corrupted unicorn corpse")]
    public sealed class SrdCorruptedUnicorn : SrdMonster
    {
        [Constructable]
        public SrdCorruptedUnicorn() : base("CorruptedUnicorn") 
        {
        }

        public SrdCorruptedUnicorn(Serial serial) : base(serial) { }
    }
}
