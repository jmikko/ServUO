using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork pugilist corpse")]
    public sealed class SrdClockworkPugilist : SrdMonster
    {
        [Constructable]
        public SrdClockworkPugilist() : base("ClockworkPugilist") 
        {
        }

        public SrdClockworkPugilist(Serial serial) : base(serial) { }
    }
}
