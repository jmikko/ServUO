using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a boggard corpse")]
    public sealed class SrdBoggard : SrdMonster
    {
        [Constructable]
        public SrdBoggard() : base("Boggard") 
        {
        }

        public SrdBoggard(Serial serial) : base(serial) { }
    }
}
