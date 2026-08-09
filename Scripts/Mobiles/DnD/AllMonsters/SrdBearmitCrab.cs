using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bearmit crab corpse")]
    public sealed class SrdBearmitCrab : SrdMonster
    {
        [Constructable]
        public SrdBearmitCrab() : base("BearmitCrab") 
        {
        }

        public SrdBearmitCrab(Serial serial) : base(serial) { }
    }
}
