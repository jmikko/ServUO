using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bucca corpse")]
    public sealed class SrdBucca : SrdMonster
    {
        [Constructable]
        public SrdBucca() : base("Bucca") 
        {
        }

        public SrdBucca(Serial serial) : base(serial) { }
    }
}
