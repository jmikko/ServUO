using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drake, ethereal corpse")]
    public sealed class SrdDrakeEthereal : SrdMonster
    {
        [Constructable]
        public SrdDrakeEthereal() : base("DrakeEthereal") 
        {
        }

        public SrdDrakeEthereal(Serial serial) : base(serial) { }
    }
}
