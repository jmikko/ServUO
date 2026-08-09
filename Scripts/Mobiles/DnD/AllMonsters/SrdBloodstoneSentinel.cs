using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bloodstone sentinel corpse")]
    public sealed class SrdBloodstoneSentinel : SrdMonster
    {
        [Constructable]
        public SrdBloodstoneSentinel() : base("BloodstoneSentinel") 
        {
        }

        public SrdBloodstoneSentinel(Serial serial) : base(serial) { }
    }
}
