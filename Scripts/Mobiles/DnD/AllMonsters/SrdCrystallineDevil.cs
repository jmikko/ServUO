using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crystalline devil corpse")]
    public sealed class SrdCrystallineDevil : SrdMonster
    {
        [Constructable]
        public SrdCrystallineDevil() : base("CrystallineDevil") 
        {
        }

        public SrdCrystallineDevil(Serial serial) : base(serial) { }
    }
}
