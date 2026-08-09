using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork scorpion corpse")]
    public sealed class SrdClockworkScorpion : SrdMonster
    {
        [Constructable]
        public SrdClockworkScorpion() : base("ClockworkScorpion") 
        {
        }

        public SrdClockworkScorpion(Serial serial) : base(serial) { }
    }
}
