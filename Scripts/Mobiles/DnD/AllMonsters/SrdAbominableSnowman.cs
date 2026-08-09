using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a abominable snowman corpse")]
    public sealed class SrdAbominableSnowman : SrdMonster
    {
        [Constructable]
        public SrdAbominableSnowman() : base("AbominableSnowman") 
        {
        }

        public SrdAbominableSnowman(Serial serial) : base(serial) { }
    }
}
