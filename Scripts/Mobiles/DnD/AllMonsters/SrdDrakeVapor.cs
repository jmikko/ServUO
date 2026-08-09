using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drake, vapor corpse")]
    public sealed class SrdDrakeVapor : SrdMonster
    {
        [Constructable]
        public SrdDrakeVapor() : base("DrakeVapor") 
        {
        }

        public SrdDrakeVapor(Serial serial) : base(serial) { }
    }
}
