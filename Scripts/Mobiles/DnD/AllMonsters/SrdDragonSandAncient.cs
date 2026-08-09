using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragon, sand ancient corpse")]
    public sealed class SrdDragonSandAncient : SrdMonster
    {
        [Constructable]
        public SrdDragonSandAncient() : base("DragonSandAncient") 
        {
        }

        public SrdDragonSandAncient(Serial serial) : base(serial) { }
    }
}
