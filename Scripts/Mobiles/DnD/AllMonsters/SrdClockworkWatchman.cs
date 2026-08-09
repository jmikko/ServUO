using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork watchman corpse")]
    public sealed class SrdClockworkWatchman : SrdMonster
    {
        [Constructable]
        public SrdClockworkWatchman() : base("ClockworkWatchman") 
        {
        }

        public SrdClockworkWatchman(Serial serial) : base(serial) { }
    }
}
