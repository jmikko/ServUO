using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bathhouse drake corpse")]
    public sealed class SrdBathhouseDrake : SrdMonster
    {
        [Constructable]
        public SrdBathhouseDrake() : base("BathhouseDrake") 
        {
        }

        public SrdBathhouseDrake(Serial serial) : base(serial) { }
    }
}
