using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a algorith corpse")]
    public sealed class SrdAlgorith : SrdMonster
    {
        [Constructable]
        public SrdAlgorith() : base("Algorith") 
        {
        }

        public SrdAlgorith(Serial serial) : base(serial) { }
    }
}
