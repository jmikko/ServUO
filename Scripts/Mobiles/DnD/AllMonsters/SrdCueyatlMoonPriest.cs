using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cueyatl moon priest corpse")]
    public sealed class SrdCueyatlMoonPriest : SrdMonster
    {
        [Constructable]
        public SrdCueyatlMoonPriest() : base("CueyatlMoonPriest") 
        {
        }

        public SrdCueyatlMoonPriest(Serial serial) : base(serial) { }
    }
}
