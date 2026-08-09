using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a druid corpse")]
    public sealed class SrdDruid : SrdMonster
    {
        [Constructable]
        public SrdDruid() : base("Druid") 
        {
        }

        public SrdDruid(Serial serial) : base(serial) { }
    }
}
