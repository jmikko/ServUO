using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bannik corpse")]
    public sealed class SrdBannik : SrdMonster
    {
        [Constructable]
        public SrdBannik() : base("Bannik") 
        {
        }

        public SrdBannik(Serial serial) : base(serial) { }
    }
}
