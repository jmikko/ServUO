using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a animated instrument, quartet corpse")]
    public sealed class SrdAnimatedInstrumentQuartet : SrdMonster
    {
        [Constructable]
        public SrdAnimatedInstrumentQuartet() : base("AnimatedInstrumentQuartet") 
        {
        }

        public SrdAnimatedInstrumentQuartet(Serial serial) : base(serial) { }
    }
}
