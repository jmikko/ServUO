using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drake, venom corpse")]
    public sealed class SrdDrakeVenom : SrdMonster
    {
        [Constructable]
        public SrdDrakeVenom() : base("DrakeVenom") 
        {
        }

        public SrdDrakeVenom(Serial serial) : base(serial) { }
    }
}
