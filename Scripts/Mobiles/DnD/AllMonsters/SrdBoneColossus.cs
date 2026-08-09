using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bone colossus corpse")]
    public sealed class SrdBoneColossus : SrdMonster
    {
        [Constructable]
        public SrdBoneColossus() : base("BoneColossus") 
        {
        }

        public SrdBoneColossus(Serial serial) : base(serial) { }
    }
}
