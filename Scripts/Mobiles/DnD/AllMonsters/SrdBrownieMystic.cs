using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a brownie mystic corpse")]
    public sealed class SrdBrownieMystic : SrdMonster
    {
        [Constructable]
        public SrdBrownieMystic() : base("BrownieMystic") 
        {
        }

        public SrdBrownieMystic(Serial serial) : base(serial) { }
    }
}
