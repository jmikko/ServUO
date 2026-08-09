using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dwarf, pike guard corpse")]
    public sealed class SrdDwarfPikeGuard : SrdMonster
    {
        [Constructable]
        public SrdDwarfPikeGuard() : base("DwarfPikeGuard") 
        {
        }

        public SrdDwarfPikeGuard(Serial serial) : base(serial) { }
    }
}
