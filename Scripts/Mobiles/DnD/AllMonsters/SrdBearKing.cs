using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bear king corpse")]
    public sealed class SrdBearKing : SrdMonster
    {
        [Constructable]
        public SrdBearKing() : base("BearKing") 
        {
        }

        public SrdBearKing(Serial serial) : base(serial) { }
    }
}
