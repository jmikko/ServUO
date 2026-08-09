using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drow corpse")]
    public sealed class SrdDrow : SrdMonster
    {
        [Constructable]
        public SrdDrow() : base("Drow") 
        {
        }

        public SrdDrow(Serial serial) : base(serial) { }
    }
}
