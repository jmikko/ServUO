using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragonleaf tree corpse")]
    public sealed class SrdDragonleafTree : SrdMonster
    {
        [Constructable]
        public SrdDragonleafTree() : base("DragonleafTree") 
        {
        }

        public SrdDragonleafTree(Serial serial) : base(serial) { }
    }
}
