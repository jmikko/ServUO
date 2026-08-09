using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alchemist corpse")]
    public sealed class SrdAlchemist : SrdMonster
    {
        [Constructable]
        public SrdAlchemist() : base("Alchemist") 
        {
        }

        public SrdAlchemist(Serial serial) : base(serial) { }
    }
}
