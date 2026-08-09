using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chupacabra corpse")]
    public sealed class SrdChupacabra : SrdMonster
    {
        [Constructable]
        public SrdChupacabra() : base("Chupacabra") 
        {
        }

        public SrdChupacabra(Serial serial) : base(serial) { }
    }
}
