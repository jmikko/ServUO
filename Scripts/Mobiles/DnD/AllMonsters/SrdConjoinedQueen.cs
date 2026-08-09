using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a conjoined queen corpse")]
    public sealed class SrdConjoinedQueen : SrdMonster
    {
        [Constructable]
        public SrdConjoinedQueen() : base("ConjoinedQueen") 
        {
        }

        public SrdConjoinedQueen(Serial serial) : base(serial) { }
    }
}
