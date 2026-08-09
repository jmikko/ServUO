using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cueyatl corpse")]
    public sealed class SrdCueyatl : SrdMonster
    {
        [Constructable]
        public SrdCueyatl() : base("Cueyatl") 
        {
        }

        public SrdCueyatl(Serial serial) : base(serial) { }
    }
}
