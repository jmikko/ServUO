using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork sentinel corpse")]
    public sealed class SrdClockworkSentinel : SrdMonster
    {
        [Constructable]
        public SrdClockworkSentinel() : base("ClockworkSentinel") 
        {
        }

        public SrdClockworkSentinel(Serial serial) : base(serial) { }
    }
}
