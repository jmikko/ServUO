using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a berstuc corpse")]
    public sealed class SrdBerstuc : SrdMonster
    {
        [Constructable]
        public SrdBerstuc() : base("Berstuc") 
        {
        }

        public SrdBerstuc(Serial serial) : base(serial) { }
    }
}
