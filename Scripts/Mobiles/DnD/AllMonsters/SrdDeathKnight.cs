using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a death knight corpse")]
    public sealed class SrdDeathKnight : SrdMonster
    {
        [Constructable]
        public SrdDeathKnight() : base("DeathKnight") 
        {
        }

        public SrdDeathKnight(Serial serial) : base(serial) { }
    }
}
