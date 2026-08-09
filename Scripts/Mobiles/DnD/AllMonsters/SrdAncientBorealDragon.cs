using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient boreal dragon corpse")]
    public sealed class SrdAncientBorealDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientBorealDragon() : base("AncientBorealDragon") 
        {
        }

        public SrdAncientBorealDragon(Serial serial) : base(serial) { }
    }
}
