using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ankheg queen corpse")]
    public sealed class SrdAnkhegQueen : SrdMonster
    {
        [Constructable]
        public SrdAnkhegQueen() : base("AnkhegQueen") 
        {
        }

        public SrdAnkhegQueen(Serial serial) : base(serial) { }
    }
}
