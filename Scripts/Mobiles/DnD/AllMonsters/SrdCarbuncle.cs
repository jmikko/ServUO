using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a carbuncle corpse")]
    public sealed class SrdCarbuncle : SrdMonster
    {
        [Constructable]
        public SrdCarbuncle() : base("Carbuncle") 
        {
        }

        public SrdCarbuncle(Serial serial) : base(serial) { }
    }
}
