using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a efreeti corpse")]
    public sealed class SrdEfreeti : SrdMonster
    {
        [Constructable]
        public SrdEfreeti() : base("Efreeti") 
        {
        }

        public SrdEfreeti(Serial serial) : base(serial) { }
    }
}
