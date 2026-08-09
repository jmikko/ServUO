using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a abominable beauty corpse")]
    public sealed class SrdAbominableBeauty : SrdMonster
    {
        [Constructable]
        public SrdAbominableBeauty() : base("AbominableBeauty") 
        {
        }

        public SrdAbominableBeauty(Serial serial) : base(serial) { }
    }
}
