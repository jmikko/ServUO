using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bear, polar corpse")]
    public sealed class SrdBearPolar : SrdMonster
    {
        [Constructable]
        public SrdBearPolar() : base("BearPolar") 
        {
        }

        public SrdBearPolar(Serial serial) : base(serial) { }
    }
}
