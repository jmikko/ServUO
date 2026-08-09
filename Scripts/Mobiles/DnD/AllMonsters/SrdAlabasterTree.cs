using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alabaster tree corpse")]
    public sealed class SrdAlabasterTree : SrdMonster
    {
        [Constructable]
        public SrdAlabasterTree() : base("AlabasterTree") 
        {
        }

        public SrdAlabasterTree(Serial serial) : base(serial) { }
    }
}
