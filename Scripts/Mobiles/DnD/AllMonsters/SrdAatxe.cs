using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a aatxe corpse")]
    public sealed class SrdAatxe : SrdMonster
    {
        [Constructable]
        public SrdAatxe() : base("Aatxe") 
        {
        }

        public SrdAatxe(Serial serial) : base(serial) { }
    }
}
