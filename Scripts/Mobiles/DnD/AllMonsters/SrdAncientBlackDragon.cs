using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient black dragon corpse")]
    public sealed class SrdAncientBlackDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientBlackDragon() : base("AncientBlackDragon") 
        {
        }

        public SrdAncientBlackDragon(Serial serial) : base(serial) { }
    }
}
