using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alpine creeper corpse")]
    public sealed class SrdAlpineCreeper : SrdMonster
    {
        [Constructable]
        public SrdAlpineCreeper() : base("AlpineCreeper") 
        {
        }

        public SrdAlpineCreeper(Serial serial) : base(serial) { }
    }
}
