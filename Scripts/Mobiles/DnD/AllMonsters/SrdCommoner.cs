using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a commoner corpse")]
    public sealed class SrdCommoner : SrdMonster
    {
        [Constructable]
        public SrdCommoner() : base("Commoner") 
        {
        }

        public SrdCommoner(Serial serial) : base(serial) { }
    }
}
