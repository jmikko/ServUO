using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork beetle corpse")]
    public sealed class SrdClockworkBeetle : SrdMonster
    {
        [Constructable]
        public SrdClockworkBeetle() : base("ClockworkBeetle") 
        {
        }

        public SrdClockworkBeetle(Serial serial) : base(serial) { }
    }
}
