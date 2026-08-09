using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dwarven ringmage corpse")]
    public sealed class SrdDwarvenRingmage : SrdMonster
    {
        [Constructable]
        public SrdDwarvenRingmage() : base("DwarvenRingmage") 
        {
        }

        public SrdDwarvenRingmage(Serial serial) : base(serial) { }
    }
}
