using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork abomination corpse")]
    public sealed class SrdClockworkAbomination : SrdMonster
    {
        [Constructable]
        public SrdClockworkAbomination() : base("ClockworkAbomination") 
        {
        }

        public SrdClockworkAbomination(Serial serial) : base(serial) { }
    }
}
