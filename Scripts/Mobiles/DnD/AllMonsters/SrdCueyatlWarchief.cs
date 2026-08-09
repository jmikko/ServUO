using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cueyatl warchief corpse")]
    public sealed class SrdCueyatlWarchief : SrdMonster
    {
        [Constructable]
        public SrdCueyatlWarchief() : base("CueyatlWarchief") 
        {
        }

        public SrdCueyatlWarchief(Serial serial) : base(serial) { }
    }
}
