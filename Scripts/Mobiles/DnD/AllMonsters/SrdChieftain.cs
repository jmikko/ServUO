using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chieftain corpse")]
    public sealed class SrdChieftain : SrdMonster
    {
        [Constructable]
        public SrdChieftain() : base("Chieftain") 
        {
        }

        public SrdChieftain(Serial serial) : base(serial) { }
    }
}
