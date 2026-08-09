using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crimson drake corpse")]
    public sealed class SrdCrimsonDrake : SrdMonster
    {
        [Constructable]
        public SrdCrimsonDrake() : base("CrimsonDrake") 
        {
        }

        public SrdCrimsonDrake(Serial serial) : base(serial) { }
    }
}
