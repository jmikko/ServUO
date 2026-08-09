using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a barometz corpse")]
    public sealed class SrdBarometz : SrdMonster
    {
        [Constructable]
        public SrdBarometz() : base("Barometz") 
        {
        }

        public SrdBarometz(Serial serial) : base(serial) { }
    }
}
