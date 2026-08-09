using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bloatblossom corpse")]
    public sealed class SrdBloatblossom : SrdMonster
    {
        [Constructable]
        public SrdBloatblossom() : base("Bloatblossom") 
        {
        }

        public SrdBloatblossom(Serial serial) : base(serial) { }
    }
}
