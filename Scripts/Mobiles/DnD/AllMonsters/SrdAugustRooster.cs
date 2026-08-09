using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a august rooster corpse")]
    public sealed class SrdAugustRooster : SrdMonster
    {
        [Constructable]
        public SrdAugustRooster() : base("AugustRooster") 
        {
        }

        public SrdAugustRooster(Serial serial) : base(serial) { }
    }
}
