using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dryad corpse")]
    public sealed class SrdDryad : SrdMonster
    {
        [Constructable]
        public SrdDryad() : base("Dryad") 
        {
        }

        public SrdDryad(Serial serial) : base(serial) { }
    }
}
