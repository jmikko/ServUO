using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crystalline monolith corpse")]
    public sealed class SrdCrystallineMonolith : SrdMonster
    {
        [Constructable]
        public SrdCrystallineMonolith() : base("CrystallineMonolith") 
        {
        }

        public SrdCrystallineMonolith(Serial serial) : base(serial) { }
    }
}
