using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a doppelrat corpse")]
    public sealed class SrdDoppelrat : SrdMonster
    {
        [Constructable]
        public SrdDoppelrat() : base("Doppelrat") 
        {
        }

        public SrdDoppelrat(Serial serial) : base(serial) { }
    }
}
