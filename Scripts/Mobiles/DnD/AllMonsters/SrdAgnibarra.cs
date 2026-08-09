using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a agnibarra corpse")]
    public sealed class SrdAgnibarra : SrdMonster
    {
        [Constructable]
        public SrdAgnibarra() : base("Agnibarra") 
        {
        }

        public SrdAgnibarra(Serial serial) : base(serial) { }
    }
}
