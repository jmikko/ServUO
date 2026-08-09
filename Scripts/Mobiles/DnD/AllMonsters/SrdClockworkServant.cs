using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork servant corpse")]
    public sealed class SrdClockworkServant : SrdMonster
    {
        [Constructable]
        public SrdClockworkServant() : base("ClockworkServant") 
        {
        }

        public SrdClockworkServant(Serial serial) : base(serial) { }
    }
}
