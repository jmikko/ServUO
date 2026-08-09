using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a angel, chained corpse")]
    public sealed class SrdAngelChained : SrdMonster
    {
        [Constructable]
        public SrdAngelChained() : base("AngelChained") 
        {
        }

        public SrdAngelChained(Serial serial) : base(serial) { }
    }
}
