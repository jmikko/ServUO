using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a duergar corpse")]
    public sealed class SrdDuergar : SrdMonster
    {
        [Constructable]
        public SrdDuergar() : base("Duergar") 
        {
        }

        public SrdDuergar(Serial serial) : base(serial) { }
    }
}
