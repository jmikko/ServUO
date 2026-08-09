using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a brain hood corpse")]
    public sealed class SrdBrainHood : SrdMonster
    {
        [Constructable]
        public SrdBrainHood() : base("BrainHood") 
        {
        }

        public SrdBrainHood(Serial serial) : base(serial) { }
    }
}
