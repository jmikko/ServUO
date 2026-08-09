using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dretch corpse")]
    public sealed class SrdDretch : SrdMonster
    {
        [Constructable]
        public SrdDretch() : base("Dretch") 
        {
        }

        public SrdDretch(Serial serial) : base(serial) { }
    }
}
