using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a coral drake corpse")]
    public sealed class SrdCoralDrake : SrdMonster
    {
        [Constructable]
        public SrdCoralDrake() : base("CoralDrake") 
        {
        }

        public SrdCoralDrake(Serial serial) : base(serial) { }
    }
}
