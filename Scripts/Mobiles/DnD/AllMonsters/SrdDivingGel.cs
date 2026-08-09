using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a diving gel corpse")]
    public sealed class SrdDivingGel : SrdMonster
    {
        [Constructable]
        public SrdDivingGel() : base("DivingGel") 
        {
        }

        public SrdDivingGel(Serial serial) : base(serial) { }
    }
}
