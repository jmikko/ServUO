using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork myrmidon corpse")]
    public sealed class SrdClockworkMyrmidon : SrdMonster
    {
        [Constructable]
        public SrdClockworkMyrmidon() : base("ClockworkMyrmidon") 
        {
        }

        public SrdClockworkMyrmidon(Serial serial) : base(serial) { }
    }
}
