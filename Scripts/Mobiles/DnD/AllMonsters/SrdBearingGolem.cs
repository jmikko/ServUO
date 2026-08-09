using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bearing golem corpse")]
    public sealed class SrdBearingGolem : SrdMonster
    {
        [Constructable]
        public SrdBearingGolem() : base("BearingGolem") 
        {
        }

        public SrdBearingGolem(Serial serial) : base(serial) { }
    }
}
