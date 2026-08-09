using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dust goblin chieftain corpse")]
    public sealed class SrdDustGoblinChieftain : SrdMonster
    {
        [Constructable]
        public SrdDustGoblinChieftain() : base("DustGoblinChieftain") 
        {
        }

        public SrdDustGoblinChieftain(Serial serial) : base(serial) { }
    }
}
