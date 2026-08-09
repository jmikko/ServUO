using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork archon corpse")]
    public sealed class SrdClockworkArchon : SrdMonster
    {
        [Constructable]
        public SrdClockworkArchon() : base("ClockworkArchon") 
        {
        }

        public SrdClockworkArchon(Serial serial) : base(serial) { }
    }
}
