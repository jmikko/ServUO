using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork leech corpse")]
    public sealed class SrdClockworkLeech : SrdMonster
    {
        [Constructable]
        public SrdClockworkLeech() : base("ClockworkLeech") 
        {
        }

        public SrdClockworkLeech(Serial serial) : base(serial) { }
    }
}
