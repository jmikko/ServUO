using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bone golem corpse")]
    public sealed class SrdBoneGolem : SrdMonster
    {
        [Constructable]
        public SrdBoneGolem() : base("BoneGolem") 
        {
        }

        public SrdBoneGolem(Serial serial) : base(serial) { }
    }
}
