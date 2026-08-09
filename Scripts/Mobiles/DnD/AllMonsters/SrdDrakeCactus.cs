using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drake, cactus corpse")]
    public sealed class SrdDrakeCactus : SrdMonster
    {
        [Constructable]
        public SrdDrakeCactus() : base("DrakeCactus") 
        {
        }

        public SrdDrakeCactus(Serial serial) : base(serial) { }
    }
}
