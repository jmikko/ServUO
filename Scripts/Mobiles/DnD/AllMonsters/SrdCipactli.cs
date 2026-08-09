using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cipactli corpse")]
    public sealed class SrdCipactli : SrdMonster
    {
        [Constructable]
        public SrdCipactli() : base("Cipactli") 
        {
        }

        public SrdCipactli(Serial serial) : base(serial) { }
    }
}
