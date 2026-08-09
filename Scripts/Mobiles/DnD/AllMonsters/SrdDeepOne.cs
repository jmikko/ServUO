using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deep one corpse")]
    public sealed class SrdDeepOne : SrdMonster
    {
        [Constructable]
        public SrdDeepOne() : base("DeepOne") 
        {
        }

        public SrdDeepOne(Serial serial) : base(serial) { }
    }
}
