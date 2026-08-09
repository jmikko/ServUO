using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cave drake corpse")]
    public sealed class SrdCaveDrake : SrdMonster
    {
        [Constructable]
        public SrdCaveDrake() : base("CaveDrake") 
        {
        }

        public SrdCaveDrake(Serial serial) : base(serial) { }
    }
}
