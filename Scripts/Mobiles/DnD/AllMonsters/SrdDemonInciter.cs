using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a demon, inciter corpse")]
    public sealed class SrdDemonInciter : SrdMonster
    {
        [Constructable]
        public SrdDemonInciter() : base("DemonInciter") 
        {
        }

        public SrdDemonInciter(Serial serial) : base(serial) { }
    }
}
