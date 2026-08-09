using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a death shroud golem corpse")]
    public sealed class SrdDeathShroudGolem : SrdMonster
    {
        [Constructable]
        public SrdDeathShroudGolem() : base("DeathShroudGolem") 
        {
        }

        public SrdDeathShroudGolem(Serial serial) : base(serial) { }
    }
}
