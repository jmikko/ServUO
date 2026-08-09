using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork beetle swarm corpse")]
    public sealed class SrdClockworkBeetleSwarm : SrdMonster
    {
        [Constructable]
        public SrdClockworkBeetleSwarm() : base("ClockworkBeetleSwarm") 
        {
        }

        public SrdClockworkBeetleSwarm(Serial serial) : base(serial) { }
    }
}
