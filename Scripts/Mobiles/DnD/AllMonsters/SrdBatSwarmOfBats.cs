using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bat, swarm of bats corpse")]
    public sealed class SrdBatSwarmOfBats : SrdMonster
    {
        [Constructable]
        public SrdBatSwarmOfBats() : base("BatSwarmOfBats") 
        {
        }

        public SrdBatSwarmOfBats(Serial serial) : base(serial) { }
    }
}
