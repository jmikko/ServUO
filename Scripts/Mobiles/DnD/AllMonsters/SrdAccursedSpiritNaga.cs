using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a accursed spirit naga corpse")]
    public sealed class SrdAccursedSpiritNaga : SrdMonster
    {
        [Constructable]
        public SrdAccursedSpiritNaga() : base("AccursedSpiritNaga") 
        {
        }

        public SrdAccursedSpiritNaga(Serial serial) : base(serial) { }
    }
}
