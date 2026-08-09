using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork conductor corpse")]
    public sealed class SrdClockworkConductor : SrdMonster
    {
        [Constructable]
        public SrdClockworkConductor() : base("ClockworkConductor") 
        {
        }

        public SrdClockworkConductor(Serial serial) : base(serial) { }
    }
}
