using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dwarf, angler corpse")]
    public sealed class SrdDwarfAngler : SrdMonster
    {
        [Constructable]
        public SrdDwarfAngler() : base("DwarfAngler") 
        {
        }

        public SrdDwarfAngler(Serial serial) : base(serial) { }
    }
}
