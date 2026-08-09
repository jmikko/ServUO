using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dimensional shambler corpse")]
    public sealed class SrdDimensionalShambler : SrdMonster
    {
        [Constructable]
        public SrdDimensionalShambler() : base("DimensionalShambler") 
        {
        }

        public SrdDimensionalShambler(Serial serial) : base(serial) { }
    }
}
