using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clacking skeleton corpse")]
    public sealed class SrdClackingSkeleton : SrdMonster
    {
        [Constructable]
        public SrdClackingSkeleton() : base("ClackingSkeleton") 
        {
        }

        public SrdClackingSkeleton(Serial serial) : base(serial) { }
    }
}
