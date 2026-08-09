using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient blue dragon corpse")]
    public sealed class SrdAncientBlueDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientBlueDragon() : base("AncientBlueDragon") 
        {
        }

        public SrdAncientBlueDragon(Serial serial) : base(serial) { }
    }
}
