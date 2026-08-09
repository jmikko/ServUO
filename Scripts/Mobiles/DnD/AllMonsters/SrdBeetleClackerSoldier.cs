using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a beetle, clacker soldier corpse")]
    public sealed class SrdBeetleClackerSoldier : SrdMonster
    {
        [Constructable]
        public SrdBeetleClackerSoldier() : base("BeetleClackerSoldier") 
        {
        }

        public SrdBeetleClackerSoldier(Serial serial) : base(serial) { }
    }
}
