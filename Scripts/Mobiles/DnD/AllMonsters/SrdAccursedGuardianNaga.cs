using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a accursed guardian naga corpse")]
    public sealed class SrdAccursedGuardianNaga : SrdMonster
    {
        [Constructable]
        public SrdAccursedGuardianNaga() : base("AccursedGuardianNaga") 
        {
        }

        public SrdAccursedGuardianNaga(Serial serial) : base(serial) { }
    }
}
