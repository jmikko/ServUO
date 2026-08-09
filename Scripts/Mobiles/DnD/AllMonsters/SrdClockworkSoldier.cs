using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork soldier corpse")]
    public sealed class SrdClockworkSoldier : SrdMonster
    {
        [Constructable]
        public SrdClockworkSoldier() : base("ClockworkSoldier") 
        {
        }

        public SrdClockworkSoldier(Serial serial) : base(serial) { }
    }
}
