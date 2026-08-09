using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a diomedian horse corpse")]
    public sealed class SrdDiomedianHorse : SrdMonster
    {
        [Constructable]
        public SrdDiomedianHorse() : base("DiomedianHorse") 
        {
        }

        public SrdDiomedianHorse(Serial serial) : base(serial) { }
    }
}
