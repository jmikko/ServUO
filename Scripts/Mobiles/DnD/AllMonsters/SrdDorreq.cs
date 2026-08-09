using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dorreq corpse")]
    public sealed class SrdDorreq : SrdMonster
    {
        [Constructable]
        public SrdDorreq() : base("Dorreq") 
        {
        }

        public SrdDorreq(Serial serial) : base(serial) { }
    }
}
