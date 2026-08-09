using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a demon, maha corpse")]
    public sealed class SrdDemonMaha : SrdMonster
    {
        [Constructable]
        public SrdDemonMaha() : base("DemonMaha") 
        {
        }

        public SrdDemonMaha(Serial serial) : base(serial) { }
    }
}
