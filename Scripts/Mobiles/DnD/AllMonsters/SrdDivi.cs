using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a divi corpse")]
    public sealed class SrdDivi : SrdMonster
    {
        [Constructable]
        public SrdDivi() : base("Divi") 
        {
        }

        public SrdDivi(Serial serial) : base(serial) { }
    }
}
