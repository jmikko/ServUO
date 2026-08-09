using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deep one archimandrite corpse")]
    public sealed class SrdDeepOneArchimandrite : SrdMonster
    {
        [Constructable]
        public SrdDeepOneArchimandrite() : base("DeepOneArchimandrite") 
        {
        }

        public SrdDeepOneArchimandrite(Serial serial) : base(serial) { }
    }
}
