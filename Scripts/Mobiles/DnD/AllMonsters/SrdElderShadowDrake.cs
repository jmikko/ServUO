using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a elder shadow drake corpse")]
    public sealed class SrdElderShadowDrake : SrdMonster
    {
        [Constructable]
        public SrdElderShadowDrake() : base("ElderShadowDrake") 
        {
        }

        public SrdElderShadowDrake(Serial serial) : base(serial) { }
    }
}
