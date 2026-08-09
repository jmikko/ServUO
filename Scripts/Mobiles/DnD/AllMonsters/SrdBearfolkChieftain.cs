using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bearfolk chieftain corpse")]
    public sealed class SrdBearfolkChieftain : SrdMonster
    {
        [Constructable]
        public SrdBearfolkChieftain() : base("BearfolkChieftain") 
        {
        }

        public SrdBearfolkChieftain(Serial serial) : base(serial) { }
    }
}
