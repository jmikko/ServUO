using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient silver dragon corpse")]
    public sealed class SrdAncientSilverDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientSilverDragon() : base("AncientSilverDragon") 
        {
        }

        public SrdAncientSilverDragon(Serial serial) : base(serial) { }
    }
}
