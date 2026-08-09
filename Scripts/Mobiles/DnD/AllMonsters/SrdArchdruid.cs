using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a archdruid corpse")]
    public sealed class SrdArchdruid : SrdMonster
    {
        [Constructable]
        public SrdArchdruid() : base("Archdruid") 
        {
        }

        public SrdArchdruid(Serial serial) : base(serial) { }
    }
}
