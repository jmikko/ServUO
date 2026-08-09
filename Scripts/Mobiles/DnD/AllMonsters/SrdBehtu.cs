using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a behtu corpse")]
    public sealed class SrdBehtu : SrdMonster
    {
        [Constructable]
        public SrdBehtu() : base("Behtu") 
        {
        }

        public SrdBehtu(Serial serial) : base(serial) { }
    }
}
