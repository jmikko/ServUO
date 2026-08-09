using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alehouse drake corpse")]
    public sealed class SrdAlehouseDrake : SrdMonster
    {
        [Constructable]
        public SrdAlehouseDrake() : base("AlehouseDrake") 
        {
        }

        public SrdAlehouseDrake(Serial serial) : base(serial) { }
    }
}
