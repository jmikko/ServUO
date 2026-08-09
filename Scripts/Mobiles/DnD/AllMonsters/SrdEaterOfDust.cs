using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a eater of dust corpse")]
    public sealed class SrdEaterOfDust : SrdMonster
    {
        [Constructable]
        public SrdEaterOfDust() : base("EaterOfDust") 
        {
        }

        public SrdEaterOfDust(Serial serial) : base(serial) { }
    }
}
