using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chaos-spawn goblin corpse")]
    public sealed class SrdChaosSpawnGoblin : SrdMonster
    {
        [Constructable]
        public SrdChaosSpawnGoblin() : base("ChaosSpawnGoblin") 
        {
        }

        public SrdChaosSpawnGoblin(Serial serial) : base(serial) { }
    }
}
