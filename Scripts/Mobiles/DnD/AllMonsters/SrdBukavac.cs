using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bukavac corpse")]
    public sealed class SrdBukavac : SrdMonster
    {
        [Constructable]
        public SrdBukavac() : base("Bukavac") 
        {
        }

        public SrdBukavac(Serial serial) : base(serial) { }
    }
}
