using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deep dwarf soldier corpse")]
    public sealed class SrdDeepDwarfSoldier : SrdMonster
    {
        [Constructable]
        public SrdDeepDwarfSoldier() : base("DeepDwarfSoldier") 
        {
        }

        public SrdDeepDwarfSoldier(Serial serial) : base(serial) { }
    }
}
