using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drake, shepherd corpse")]
    public sealed class SrdDrakeShepherd : SrdMonster
    {
        [Constructable]
        public SrdDrakeShepherd() : base("DrakeShepherd") 
        {
        }

        public SrdDrakeShepherd(Serial serial) : base(serial) { }
    }
}
