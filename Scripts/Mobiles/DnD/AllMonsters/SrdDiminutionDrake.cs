using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a diminution drake corpse")]
    public sealed class SrdDiminutionDrake : SrdMonster
    {
        [Constructable]
        public SrdDiminutionDrake() : base("DiminutionDrake") 
        {
        }

        public SrdDiminutionDrake(Serial serial) : base(serial) { }
    }
}
