using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cacus giant corpse")]
    public sealed class SrdCacusGiant : SrdMonster
    {
        [Constructable]
        public SrdCacusGiant() : base("CacusGiant") 
        {
        }

        public SrdCacusGiant(Serial serial) : base(serial) { }
    }
}
