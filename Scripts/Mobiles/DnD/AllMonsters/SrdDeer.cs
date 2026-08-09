using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deer corpse")]
    public sealed class SrdDeer : SrdMonster
    {
        [Constructable]
        public SrdDeer() : base("Deer") 
        {
        }

        public SrdDeer(Serial serial) : base(serial) { }
    }
}
