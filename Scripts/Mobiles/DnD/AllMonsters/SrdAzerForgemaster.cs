using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a azer forgemaster corpse")]
    public sealed class SrdAzerForgemaster : SrdMonster
    {
        [Constructable]
        public SrdAzerForgemaster() : base("AzerForgemaster") 
        {
        }

        public SrdAzerForgemaster(Serial serial) : base(serial) { }
    }
}
