using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a animated instrument corpse")]
    public sealed class SrdAnimatedInstrument : SrdMonster
    {
        [Constructable]
        public SrdAnimatedInstrument() : base("AnimatedInstrument") 
        {
        }

        public SrdAnimatedInstrument(Serial serial) : base(serial) { }
    }
}
