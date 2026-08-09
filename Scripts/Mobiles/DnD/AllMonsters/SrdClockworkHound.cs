using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork hound corpse")]
    public sealed class SrdClockworkHound : SrdMonster
    {
        [Constructable]
        public SrdClockworkHound() : base("ClockworkHound") 
        {
        }

        public SrdClockworkHound(Serial serial) : base(serial) { }
    }
}
