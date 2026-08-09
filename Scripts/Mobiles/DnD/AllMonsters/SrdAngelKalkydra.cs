using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a angel, kalkydra corpse")]
    public sealed class SrdAngelKalkydra : SrdMonster
    {
        [Constructable]
        public SrdAngelKalkydra() : base("AngelKalkydra") 
        {
        }

        public SrdAngelKalkydra(Serial serial) : base(serial) { }
    }
}
