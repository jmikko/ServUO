using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a barong corpse")]
    public sealed class SrdBarong : SrdMonster
    {
        [Constructable]
        public SrdBarong() : base("Barong") 
        {
        }

        public SrdBarong(Serial serial) : base(serial) { }
    }
}
