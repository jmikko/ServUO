using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a amber ooze corpse")]
    public sealed class SrdAmberOoze : SrdMonster
    {
        [Constructable]
        public SrdAmberOoze() : base("AmberOoze") 
        {
        }

        public SrdAmberOoze(Serial serial) : base(serial) { }
    }
}
