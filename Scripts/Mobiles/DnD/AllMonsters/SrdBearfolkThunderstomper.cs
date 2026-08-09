using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bearfolk thunderstomper corpse")]
    public sealed class SrdBearfolkThunderstomper : SrdMonster
    {
        [Constructable]
        public SrdBearfolkThunderstomper() : base("BearfolkThunderstomper") 
        {
        }

        public SrdBearfolkThunderstomper(Serial serial) : base(serial) { }
    }
}
