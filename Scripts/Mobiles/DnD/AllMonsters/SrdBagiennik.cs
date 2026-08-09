using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bagiennik corpse")]
    public sealed class SrdBagiennik : SrdMonster
    {
        [Constructable]
        public SrdBagiennik() : base("Bagiennik") 
        {
        }

        public SrdBagiennik(Serial serial) : base(serial) { }
    }
}
