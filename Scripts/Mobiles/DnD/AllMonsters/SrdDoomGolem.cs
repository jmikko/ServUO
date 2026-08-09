using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a doom golem corpse")]
    public sealed class SrdDoomGolem : SrdMonster
    {
        [Constructable]
        public SrdDoomGolem() : base("DoomGolem") 
        {
        }

        public SrdDoomGolem(Serial serial) : base(serial) { }
    }
}
