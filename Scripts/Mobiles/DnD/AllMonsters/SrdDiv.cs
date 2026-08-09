using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a div corpse")]
    public sealed class SrdDiv : SrdMonster
    {
        [Constructable]
        public SrdDiv() : base("Div") 
        {
        }

        public SrdDiv(Serial serial) : base(serial) { }
    }
}
