using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bear, black corpse")]
    public sealed class SrdBearBlack : SrdMonster
    {
        [Constructable]
        public SrdBearBlack() : base("BearBlack") 
        {
        }

        public SrdBearBlack(Serial serial) : base(serial) { }
    }
}
