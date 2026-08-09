using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a brownie corpse")]
    public sealed class SrdBrownie : SrdMonster
    {
        [Constructable]
        public SrdBrownie() : base("Brownie") 
        {
        }

        public SrdBrownie(Serial serial) : base(serial) { }
    }
}
