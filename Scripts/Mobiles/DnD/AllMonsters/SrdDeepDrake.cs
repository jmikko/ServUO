using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deep drake corpse")]
    public sealed class SrdDeepDrake : SrdMonster
    {
        [Constructable]
        public SrdDeepDrake() : base("DeepDrake") 
        {
        }

        public SrdDeepDrake(Serial serial) : base(serial) { }
    }
}
