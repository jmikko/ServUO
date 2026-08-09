using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bloody bones corpse")]
    public sealed class SrdBloodyBones : SrdMonster
    {
        [Constructable]
        public SrdBloodyBones() : base("BloodyBones") 
        {
        }

        public SrdBloodyBones(Serial serial) : base(serial) { }
    }
}
