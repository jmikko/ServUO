using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a derendian moth abomination corpse")]
    public sealed class SrdDerendianMothAbomination : SrdMonster
    {
        [Constructable]
        public SrdDerendianMothAbomination() : base("DerendianMothAbomination") 
        {
        }

        public SrdDerendianMothAbomination(Serial serial) : base(serial) { }
    }
}
