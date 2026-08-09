using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork assassin corpse")]
    public sealed class SrdClockworkAssassin : SrdMonster
    {
        [Constructable]
        public SrdClockworkAssassin() : base("ClockworkAssassin") 
        {
        }

        public SrdClockworkAssassin(Serial serial) : base(serial) { }
    }
}
