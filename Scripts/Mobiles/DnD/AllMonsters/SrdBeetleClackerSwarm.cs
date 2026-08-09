using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a beetle, clacker swarm corpse")]
    public sealed class SrdBeetleClackerSwarm : SrdMonster
    {
        [Constructable]
        public SrdBeetleClackerSwarm() : base("BeetleClackerSwarm") 
        {
        }

        public SrdBeetleClackerSwarm(Serial serial) : base(serial) { }
    }
}
