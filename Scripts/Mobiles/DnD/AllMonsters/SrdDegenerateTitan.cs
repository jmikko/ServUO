using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a degenerate titan corpse")]
    public sealed class SrdDegenerateTitan : SrdMonster
    {
        [Constructable]
        public SrdDegenerateTitan() : base("DegenerateTitan") 
        {
        }

        public SrdDegenerateTitan(Serial serial) : base(serial) { }
    }
}
