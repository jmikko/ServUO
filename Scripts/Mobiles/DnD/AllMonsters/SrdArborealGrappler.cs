using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a arboreal grappler corpse")]
    public sealed class SrdArborealGrappler : SrdMonster
    {
        [Constructable]
        public SrdArborealGrappler() : base("ArborealGrappler") 
        {
        }

        public SrdArborealGrappler(Serial serial) : base(serial) { }
    }
}
