using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a buraq corpse")]
    public sealed class SrdBuraq : SrdMonster
    {
        [Constructable]
        public SrdBuraq() : base("Buraq") 
        {
        }

        public SrdBuraq(Serial serial) : base(serial) { }
    }
}
