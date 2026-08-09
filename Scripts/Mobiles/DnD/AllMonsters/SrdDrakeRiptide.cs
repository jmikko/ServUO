using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drake, riptide corpse")]
    public sealed class SrdDrakeRiptide : SrdMonster
    {
        [Constructable]
        public SrdDrakeRiptide() : base("DrakeRiptide") 
        {
        }

        public SrdDrakeRiptide(Serial serial) : base(serial) { }
    }
}
