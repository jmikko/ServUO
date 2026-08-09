using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a belu corpse")]
    public sealed class SrdBelu : SrdMonster
    {
        [Constructable]
        public SrdBelu() : base("Belu") 
        {
        }

        public SrdBelu(Serial serial) : base(serial) { }
    }
}
