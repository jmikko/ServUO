using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork tactician corpse")]
    public sealed class SrdClockworkTactician : SrdMonster
    {
        [Constructable]
        public SrdClockworkTactician() : base("ClockworkTactician") 
        {
        }

        public SrdClockworkTactician(Serial serial) : base(serial) { }
    }
}
