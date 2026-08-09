using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a angel, shrouded corpse")]
    public sealed class SrdAngelShrouded : SrdMonster
    {
        [Constructable]
        public SrdAngelShrouded() : base("AngelShrouded") 
        {
        }

        public SrdAngelShrouded(Serial serial) : base(serial) { }
    }
}
