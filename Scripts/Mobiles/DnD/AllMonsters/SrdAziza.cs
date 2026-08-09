using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aziza corpse")]
    public sealed class SrdAziza : SrdMonster
    {
        [Constructable]
        public SrdAziza() : base("Aziza") 
        {
        }

        public SrdAziza(Serial serial) : base(serial) { }
    }
}
