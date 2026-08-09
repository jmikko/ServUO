using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drake, reef corpse")]
    public sealed class SrdDrakeReef : SrdMonster
    {
        [Constructable]
        public SrdDrakeReef() : base("DrakeReef") 
        {
        }

        public SrdDrakeReef(Serial serial) : base(serial) { }
    }
}
