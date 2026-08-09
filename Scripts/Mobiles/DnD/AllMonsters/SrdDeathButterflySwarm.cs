using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a death butterfly swarm corpse")]
    public sealed class SrdDeathButterflySwarm : SrdMonster
    {
        [Constructable]
        public SrdDeathButterflySwarm() : base("DeathButterflySwarm") 
        {
        }

        public SrdDeathButterflySwarm(Serial serial) : base(serial) { }
    }
}
