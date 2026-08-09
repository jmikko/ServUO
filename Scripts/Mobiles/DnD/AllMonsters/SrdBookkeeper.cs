using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bookkeeper corpse")]
    public sealed class SrdBookkeeper : SrdMonster
    {
        [Constructable]
        public SrdBookkeeper() : base("Bookkeeper") 
        {
        }

        public SrdBookkeeper(Serial serial) : base(serial) { }
    }
}
