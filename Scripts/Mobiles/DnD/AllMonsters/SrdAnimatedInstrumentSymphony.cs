using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a animated instrument, symphony corpse")]
    public sealed class SrdAnimatedInstrumentSymphony : SrdMonster
    {
        [Constructable]
        public SrdAnimatedInstrumentSymphony() : base("AnimatedInstrumentSymphony") 
        {
        }

        public SrdAnimatedInstrumentSymphony(Serial serial) : base(serial) { }
    }
}
