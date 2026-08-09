using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a commoner mob corpse")]
    public sealed class SrdCommonerMob : SrdMonster
    {
        [Constructable]
        public SrdCommonerMob() : base("CommonerMob") 
        {
        }

        public SrdCommonerMob(Serial serial) : base(serial) { }
    }
}
