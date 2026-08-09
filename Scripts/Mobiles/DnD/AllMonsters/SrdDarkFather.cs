using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dark father corpse")]
    public sealed class SrdDarkFather : SrdMonster
    {
        [Constructable]
        public SrdDarkFather() : base("DarkFather") 
        {
        }

        public SrdDarkFather(Serial serial) : base(serial) { }
    }
}
