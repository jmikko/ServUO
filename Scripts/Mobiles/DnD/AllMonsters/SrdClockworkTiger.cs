using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork tiger corpse")]
    public sealed class SrdClockworkTiger : SrdMonster
    {
        [Constructable]
        public SrdClockworkTiger() : base("ClockworkTiger") 
        {
        }

        public SrdClockworkTiger(Serial serial) : base(serial) { }
    }
}
