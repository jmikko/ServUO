using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a darkmantle corpse")]
    public sealed class SrdDarkmantle : SrdMonster
    {
        [Constructable]
        public SrdDarkmantle() : base("Darkmantle") 
        {
        }

        public SrdDarkmantle(Serial serial) : base(serial) { }
    }
}
