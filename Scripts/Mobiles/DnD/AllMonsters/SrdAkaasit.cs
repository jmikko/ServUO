using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a akaasit corpse")]
    public sealed class SrdAkaasit : SrdMonster
    {
        [Constructable]
        public SrdAkaasit() : base("Akaasit") 
        {
        }

        public SrdAkaasit(Serial serial) : base(serial) { }
    }
}
