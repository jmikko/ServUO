using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a de ogen corpse")]
    public sealed class SrdDeOgen : SrdMonster
    {
        [Constructable]
        public SrdDeOgen() : base("DeOgen") 
        {
        }

        public SrdDeOgen(Serial serial) : base(serial) { }
    }
}
