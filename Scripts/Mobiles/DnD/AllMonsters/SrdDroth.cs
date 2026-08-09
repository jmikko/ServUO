using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a droth corpse")]
    public sealed class SrdDroth : SrdMonster
    {
        [Constructable]
        public SrdDroth() : base("Droth") 
        {
        }

        public SrdDroth(Serial serial) : base(serial) { }
    }
}
