using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crimson shambler corpse")]
    public sealed class SrdCrimsonShambler : SrdMonster
    {
        [Constructable]
        public SrdCrimsonShambler() : base("CrimsonShambler") 
        {
        }

        public SrdCrimsonShambler(Serial serial) : base(serial) { }
    }
}
