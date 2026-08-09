using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a caustic charger corpse")]
    public sealed class SrdCausticCharger : SrdMonster
    {
        [Constructable]
        public SrdCausticCharger() : base("CausticCharger") 
        {
        }

        public SrdCausticCharger(Serial serial) : base(serial) { }
    }
}
