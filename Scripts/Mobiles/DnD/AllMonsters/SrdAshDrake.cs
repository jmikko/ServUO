using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ash drake corpse")]
    public sealed class SrdAshDrake : SrdMonster
    {
        [Constructable]
        public SrdAshDrake() : base("AshDrake") 
        {
        }

        public SrdAshDrake(Serial serial) : base(serial) { }
    }
}
