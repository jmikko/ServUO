using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dwarf, pike guard captain corpse")]
    public sealed class SrdDwarfPikeGuardCaptain : SrdMonster
    {
        [Constructable]
        public SrdDwarfPikeGuardCaptain() : base("DwarfPikeGuardCaptain") 
        {
        }

        public SrdDwarfPikeGuardCaptain(Serial serial) : base(serial) { }
    }
}
