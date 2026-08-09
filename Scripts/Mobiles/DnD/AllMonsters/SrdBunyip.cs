using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bunyip corpse")]
    public sealed class SrdBunyip : SrdMonster
    {
        [Constructable]
        public SrdBunyip() : base("Bunyip") 
        {
        }

        public SrdBunyip(Serial serial) : base(serial) { }
    }
}
