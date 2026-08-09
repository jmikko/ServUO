using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a brownie beastrider corpse")]
    public sealed class SrdBrownieBeastrider : SrdMonster
    {
        [Constructable]
        public SrdBrownieBeastrider() : base("BrownieBeastrider") 
        {
        }

        public SrdBrownieBeastrider(Serial serial) : base(serial) { }
    }
}
