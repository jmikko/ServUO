using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dhampir corpse")]
    public sealed class SrdDhampir : SrdMonster
    {
        [Constructable]
        public SrdDhampir() : base("Dhampir") 
        {
        }

        public SrdDhampir(Serial serial) : base(serial) { }
    }
}
