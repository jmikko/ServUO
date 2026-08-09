using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient gold dragon corpse")]
    public sealed class SrdAncientGoldDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientGoldDragon() : base("AncientGoldDragon") 
        {
        }

        public SrdAncientGoldDragon(Serial serial) : base(serial) { }
    }
}
