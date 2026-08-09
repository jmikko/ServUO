using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bearded devil corpse")]
    public sealed class SrdBeardedDevil : SrdMonster
    {
        [Constructable]
        public SrdBeardedDevil() : base("BeardedDevil") 
        {
        }

        public SrdBeardedDevil(Serial serial) : base(serial) { }
    }
}
