using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deathcap myconid corpse")]
    public sealed class SrdDeathcapMyconid : SrdMonster
    {
        [Constructable]
        public SrdDeathcapMyconid() : base("DeathcapMyconid") 
        {
        }

        public SrdDeathcapMyconid(Serial serial) : base(serial) { }
    }
}
