using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient emerald dragon corpse")]
    public sealed class SrdAncientEmeraldDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientEmeraldDragon() : base("AncientEmeraldDragon") 
        {
        }

        public SrdAncientEmeraldDragon(Serial serial) : base(serial) { }
    }
}
