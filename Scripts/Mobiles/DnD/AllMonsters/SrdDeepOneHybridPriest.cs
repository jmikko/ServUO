using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deep one hybrid priest corpse")]
    public sealed class SrdDeepOneHybridPriest : SrdMonster
    {
        [Constructable]
        public SrdDeepOneHybridPriest() : base("DeepOneHybridPriest") 
        {
        }

        public SrdDeepOneHybridPriest(Serial serial) : base(serial) { }
    }
}
