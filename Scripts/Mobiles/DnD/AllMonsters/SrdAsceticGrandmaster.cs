using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ascetic grandmaster corpse")]
    public sealed class SrdAsceticGrandmaster : SrdMonster
    {
        [Constructable]
        public SrdAsceticGrandmaster() : base("AsceticGrandmaster") 
        {
        }

        public SrdAsceticGrandmaster(Serial serial) : base(serial) { }
    }
}
