using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a beli corpse")]
    public sealed class SrdBeli : SrdMonster
    {
        [Constructable]
        public SrdBeli() : base("Beli") 
        {
        }

        public SrdBeli(Serial serial) : base(serial) { }
    }
}
