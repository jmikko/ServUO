using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bearfolk corpse")]
    public sealed class SrdBearfolk : SrdMonster
    {
        [Constructable]
        public SrdBearfolk() : base("Bearfolk") 
        {
        }

        public SrdBearfolk(Serial serial) : base(serial) { }
    }
}
