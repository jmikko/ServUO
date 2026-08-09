using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragon, sand adult corpse")]
    public sealed class SrdDragonSandAdult : SrdMonster
    {
        [Constructable]
        public SrdDragonSandAdult() : base("DragonSandAdult") 
        {
        }

        public SrdDragonSandAdult(Serial serial) : base(serial) { }
    }
}
