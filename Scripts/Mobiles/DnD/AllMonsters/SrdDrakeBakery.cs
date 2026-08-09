using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drake, bakery corpse")]
    public sealed class SrdDrakeBakery : SrdMonster
    {
        [Constructable]
        public SrdDrakeBakery() : base("DrakeBakery") 
        {
        }

        public SrdDrakeBakery(Serial serial) : base(serial) { }
    }
}
