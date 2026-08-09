using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a duelist corpse")]
    public sealed class SrdDuelist : SrdMonster
    {
        [Constructable]
        public SrdDuelist() : base("Duelist") 
        {
        }

        public SrdDuelist(Serial serial) : base(serial) { }
    }
}
