using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bandit lord corpse")]
    public sealed class SrdBanditLord : SrdMonster
    {
        [Constructable]
        public SrdBanditLord() : base("BanditLord") 
        {
        }

        public SrdBanditLord(Serial serial) : base(serial) { }
    }
}
