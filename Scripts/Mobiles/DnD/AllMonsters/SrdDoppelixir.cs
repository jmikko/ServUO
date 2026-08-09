using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a doppelixir corpse")]
    public sealed class SrdDoppelixir : SrdMonster
    {
        [Constructable]
        public SrdDoppelixir() : base("Doppelixir") 
        {
        }

        public SrdDoppelixir(Serial serial) : base(serial) { }
    }
}
