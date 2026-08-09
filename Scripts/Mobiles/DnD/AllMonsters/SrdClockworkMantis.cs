using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork mantis corpse")]
    public sealed class SrdClockworkMantis : SrdMonster
    {
        [Constructable]
        public SrdClockworkMantis() : base("ClockworkMantis") 
        {
        }

        public SrdClockworkMantis(Serial serial) : base(serial) { }
    }
}
