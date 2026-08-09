using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient white dragon corpse")]
    public sealed class SrdAncientWhiteDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientWhiteDragon() : base("AncientWhiteDragon") 
        {
        }

        public SrdAncientWhiteDragon(Serial serial) : base(serial) { }
    }
}
