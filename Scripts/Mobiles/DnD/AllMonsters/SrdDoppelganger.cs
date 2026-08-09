using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a doppelganger corpse")]
    public sealed class SrdDoppelganger : SrdMonster
    {
        [Constructable]
        public SrdDoppelganger() : base("Doppelganger") 
        {
        }

        public SrdDoppelganger(Serial serial) : base(serial) { }
    }
}
