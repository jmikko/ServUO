using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient copper dragon corpse")]
    public sealed class SrdAncientCopperDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientCopperDragon() : base("AncientCopperDragon") 
        {
        }

        public SrdAncientCopperDragon(Serial serial) : base(serial) { }
    }
}
