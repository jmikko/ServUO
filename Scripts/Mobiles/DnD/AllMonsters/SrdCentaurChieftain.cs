using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a centaur chieftain corpse")]
    public sealed class SrdCentaurChieftain : SrdMonster
    {
        [Constructable]
        public SrdCentaurChieftain() : base("CentaurChieftain") 
        {
        }

        public SrdCentaurChieftain(Serial serial) : base(serial) { }
    }
}
