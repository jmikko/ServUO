using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dwarf, firecracker corpse")]
    public sealed class SrdDwarfFirecracker : SrdMonster
    {
        [Constructable]
        public SrdDwarfFirecracker() : base("DwarfFirecracker") 
        {
        }

        public SrdDwarfFirecracker(Serial serial) : base(serial) { }
    }
}
