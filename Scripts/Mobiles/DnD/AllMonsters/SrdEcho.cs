using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a echo corpse")]
    public sealed class SrdEcho : SrdMonster
    {
        [Constructable]
        public SrdEcho() : base("Echo") 
        {
        }

        public SrdEcho(Serial serial) : base(serial) { }
    }
}
