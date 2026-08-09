using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a a-mi-kuk corpse")]
    public sealed class SrdAMiKuk : SrdMonster
    {
        [Constructable]
        public SrdAMiKuk() : base("AMiKuk") 
        {
        }

        public SrdAMiKuk(Serial serial) : base(serial) { }
    }
}
