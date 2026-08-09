using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bard corpse")]
    public sealed class SrdBard : SrdMonster
    {
        [Constructable]
        public SrdBard() : base("Bard") 
        {
        }

        public SrdBard(Serial serial) : base(serial) { }
    }
}
