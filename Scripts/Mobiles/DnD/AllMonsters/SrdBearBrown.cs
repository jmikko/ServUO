using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bear, brown corpse")]
    public sealed class SrdBearBrown : SrdMonster
    {
        [Constructable]
        public SrdBearBrown() : base("BearBrown") 
        {
        }

        public SrdBearBrown(Serial serial) : base(serial) { }
    }
}
