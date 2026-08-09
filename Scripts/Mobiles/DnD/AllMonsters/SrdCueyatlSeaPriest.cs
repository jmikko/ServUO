using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cueyatl sea priest corpse")]
    public sealed class SrdCueyatlSeaPriest : SrdMonster
    {
        [Constructable]
        public SrdCueyatlSeaPriest() : base("CueyatlSeaPriest") 
        {
        }

        public SrdCueyatlSeaPriest(Serial serial) : base(serial) { }
    }
}
