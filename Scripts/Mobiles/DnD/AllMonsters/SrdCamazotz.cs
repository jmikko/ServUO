using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a camazotz corpse")]
    public sealed class SrdCamazotz : SrdMonster
    {
        [Constructable]
        public SrdCamazotz() : base("Camazotz") 
        {
        }

        public SrdCamazotz(Serial serial) : base(serial) { }
    }
}
