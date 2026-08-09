using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a elemental locus corpse")]
    public sealed class SrdElementalLocus : SrdMonster
    {
        [Constructable]
        public SrdElementalLocus() : base("ElementalLocus") 
        {
        }

        public SrdElementalLocus(Serial serial) : base(serial) { }
    }
}
