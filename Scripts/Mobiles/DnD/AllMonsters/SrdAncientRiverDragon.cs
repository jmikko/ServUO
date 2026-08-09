using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient river dragon corpse")]
    public sealed class SrdAncientRiverDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientRiverDragon() : base("AncientRiverDragon") 
        {
        }

        public SrdAncientRiverDragon(Serial serial) : base(serial) { }
    }
}
