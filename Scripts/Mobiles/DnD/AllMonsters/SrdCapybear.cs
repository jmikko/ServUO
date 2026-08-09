using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a capybear corpse")]
    public sealed class SrdCapybear : SrdMonster
    {
        [Constructable]
        public SrdCapybear() : base("Capybear") 
        {
        }

        public SrdCapybear(Serial serial) : base(serial) { }
    }
}
