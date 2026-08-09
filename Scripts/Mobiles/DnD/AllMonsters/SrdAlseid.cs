using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alseid corpse")]
    public sealed class SrdAlseid : SrdMonster
    {
        [Constructable]
        public SrdAlseid() : base("Alseid") 
        {
        }

        public SrdAlseid(Serial serial) : base(serial) { }
    }
}
