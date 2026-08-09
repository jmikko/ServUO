using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a divi noble corpse")]
    public sealed class SrdDiviNoble : SrdMonster
    {
        [Constructable]
        public SrdDiviNoble() : base("DiviNoble") 
        {
        }

        public SrdDiviNoble(Serial serial) : base(serial) { }
    }
}
