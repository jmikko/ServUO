using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a avulzor corpse")]
    public sealed class SrdAvulzor : SrdMonster
    {
        [Constructable]
        public SrdAvulzor() : base("Avulzor") 
        {
        }

        public SrdAvulzor(Serial serial) : base(serial) { }
    }
}
