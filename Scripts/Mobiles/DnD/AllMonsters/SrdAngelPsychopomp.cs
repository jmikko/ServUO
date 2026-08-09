using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a angel, psychopomp corpse")]
    public sealed class SrdAngelPsychopomp : SrdMonster
    {
        [Constructable]
        public SrdAngelPsychopomp() : base("AngelPsychopomp") 
        {
        }

        public SrdAngelPsychopomp(Serial serial) : base(serial) { }
    }
}
