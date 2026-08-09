using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bilwis corpse")]
    public sealed class SrdBilwis : SrdMonster
    {
        [Constructable]
        public SrdBilwis() : base("Bilwis") 
        {
        }

        public SrdBilwis(Serial serial) : base(serial) { }
    }
}
