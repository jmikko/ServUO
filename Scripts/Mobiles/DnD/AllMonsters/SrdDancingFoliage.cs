using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dancing foliage corpse")]
    public sealed class SrdDancingFoliage : SrdMonster
    {
        [Constructable]
        public SrdDancingFoliage() : base("DancingFoliage") 
        {
        }

        public SrdDancingFoliage(Serial serial) : base(serial) { }
    }
}
