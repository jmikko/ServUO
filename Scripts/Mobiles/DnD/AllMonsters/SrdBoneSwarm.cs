using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bone swarm corpse")]
    public sealed class SrdBoneSwarm : SrdMonster
    {
        [Constructable]
        public SrdBoneSwarm() : base("BoneSwarm") 
        {
        }

        public SrdBoneSwarm(Serial serial) : base(serial) { }
    }
}
