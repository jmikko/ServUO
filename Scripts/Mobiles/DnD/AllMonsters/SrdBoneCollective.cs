using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bone collective corpse")]
    public sealed class SrdBoneCollective : SrdMonster
    {
        [Constructable]
        public SrdBoneCollective() : base("BoneCollective") 
        {
        }

        public SrdBoneCollective(Serial serial) : base(serial) { }
    }
}
