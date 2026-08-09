using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult emerald dragon corpse")]
    public sealed class SrdAdultEmeraldDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultEmeraldDragon() : base("AdultEmeraldDragon") 
        {
        }

        public SrdAdultEmeraldDragon(Serial serial) : base(serial) { }
    }
}
