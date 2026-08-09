using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ala corpse")]
    public sealed class SrdAla : SrdMonster
    {
        [Constructable]
        public SrdAla() : base("Ala") 
        {
        }

        public SrdAla(Serial serial) : base(serial) { }
    }
}
