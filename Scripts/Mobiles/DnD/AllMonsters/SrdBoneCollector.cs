using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bone collector corpse")]
    public sealed class SrdBoneCollector : SrdMonster
    {
        [Constructable]
        public SrdBoneCollector() : base("BoneCollector") 
        {
        }

        public SrdBoneCollector(Serial serial) : base(serial) { }
    }
}
