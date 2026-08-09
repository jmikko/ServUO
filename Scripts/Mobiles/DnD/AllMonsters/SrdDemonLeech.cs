using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a demon, leech corpse")]
    public sealed class SrdDemonLeech : SrdMonster
    {
        [Constructable]
        public SrdDemonLeech() : base("DemonLeech") 
        {
        }

        public SrdDemonLeech(Serial serial) : base(serial) { }
    }
}
