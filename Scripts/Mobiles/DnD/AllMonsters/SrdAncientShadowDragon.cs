using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient shadow dragon corpse")]
    public sealed class SrdAncientShadowDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientShadowDragon() : base("AncientShadowDragon") 
        {
        }

        public SrdAncientShadowDragon(Serial serial) : base(serial) { }
    }
}
