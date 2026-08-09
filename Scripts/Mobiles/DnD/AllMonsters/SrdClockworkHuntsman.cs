using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork huntsman corpse")]
    public sealed class SrdClockworkHuntsman : SrdMonster
    {
        [Constructable]
        public SrdClockworkHuntsman() : base("ClockworkHuntsman") 
        {
        }

        public SrdClockworkHuntsman(Serial serial) : base(serial) { }
    }
}
