using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a brachyura shambler corpse")]
    public sealed class SrdBrachyuraShambler : SrdMonster
    {
        [Constructable]
        public SrdBrachyuraShambler() : base("BrachyuraShambler") 
        {
        }

        public SrdBrachyuraShambler(Serial serial) : base(serial) { }
    }
}
