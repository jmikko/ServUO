using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cackling skeleton corpse")]
    public sealed class SrdCacklingSkeleton : SrdMonster
    {
        [Constructable]
        public SrdCacklingSkeleton() : base("CacklingSkeleton") 
        {
        }

        public SrdCacklingSkeleton(Serial serial) : base(serial) { }
    }
}
