using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corrupting ooze corpse")]
    public sealed class SrdCorruptingOoze : SrdMonster
    {
        [Constructable]
        public SrdCorruptingOoze() : base("CorruptingOoze") 
        {
        }

        public SrdCorruptingOoze(Serial serial) : base(serial) { }
    }
}
