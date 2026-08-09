using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a couatl corpse")]
    public sealed class SrdCouatl : SrdMonster
    {
        [Constructable]
        public SrdCouatl() : base("Couatl") 
        {
        }

        public SrdCouatl(Serial serial) : base(serial) { }
    }
}
