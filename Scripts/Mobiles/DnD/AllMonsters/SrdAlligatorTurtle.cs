using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alligator turtle corpse")]
    public sealed class SrdAlligatorTurtle : SrdMonster
    {
        [Constructable]
        public SrdAlligatorTurtle() : base("AlligatorTurtle") 
        {
        }

        public SrdAlligatorTurtle(Serial serial) : base(serial) { }
    }
}
