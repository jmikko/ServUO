using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bone lord corpse")]
    public sealed class SrdBoneLord : SrdMonster
    {
        [Constructable]
        public SrdBoneLord() : base("BoneLord") 
        {
        }

        public SrdBoneLord(Serial serial) : base(serial) { }
    }
}
