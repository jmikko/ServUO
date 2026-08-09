using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a boneshard wraith corpse")]
    public sealed class SrdBoneshardWraith : SrdMonster
    {
        [Constructable]
        public SrdBoneshardWraith() : base("BoneshardWraith") 
        {
        }

        public SrdBoneshardWraith(Serial serial) : base(serial) { }
    }
}
