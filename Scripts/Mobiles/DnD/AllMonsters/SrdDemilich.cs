using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a demilich corpse")]
    public sealed class SrdDemilich : SrdMonster
    {
        [Constructable]
        public SrdDemilich() : base("Demilich") 
        {
        }

        public SrdDemilich(Serial serial) : base(serial) { }
    }
}
