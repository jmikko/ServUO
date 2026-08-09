using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drowned maiden corpse")]
    public sealed class SrdDrownedMaiden : SrdMonster
    {
        [Constructable]
        public SrdDrownedMaiden() : base("DrownedMaiden") 
        {
        }

        public SrdDrownedMaiden(Serial serial) : base(serial) { }
    }
}
