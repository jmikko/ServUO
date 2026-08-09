using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dire owlbear corpse")]
    public sealed class SrdDireOwlbear : SrdMonster
    {
        [Constructable]
        public SrdDireOwlbear() : base("DireOwlbear") 
        {
        }

        public SrdDireOwlbear(Serial serial) : base(serial) { }
    }
}
