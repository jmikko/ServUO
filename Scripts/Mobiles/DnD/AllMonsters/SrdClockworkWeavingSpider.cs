using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork weaving spider corpse")]
    public sealed class SrdClockworkWeavingSpider : SrdMonster
    {
        [Constructable]
        public SrdClockworkWeavingSpider() : base("ClockworkWeavingSpider") 
        {
        }

        public SrdClockworkWeavingSpider(Serial serial) : base(serial) { }
    }
}
