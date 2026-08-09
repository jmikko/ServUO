using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a birgemon seal corpse")]
    public sealed class SrdBirgemonSeal : SrdMonster
    {
        [Constructable]
        public SrdBirgemonSeal() : base("BirgemonSeal") 
        {
        }

        public SrdBirgemonSeal(Serial serial) : base(serial) { }
    }
}
