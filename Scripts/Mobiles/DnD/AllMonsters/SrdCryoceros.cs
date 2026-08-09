using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cryoceros corpse")]
    public sealed class SrdCryoceros : SrdMonster
    {
        [Constructable]
        public SrdCryoceros() : base("Cryoceros") 
        {
        }

        public SrdCryoceros(Serial serial) : base(serial) { }
    }
}
