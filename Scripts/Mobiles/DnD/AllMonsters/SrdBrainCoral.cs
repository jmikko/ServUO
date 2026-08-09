using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a brain coral corpse")]
    public sealed class SrdBrainCoral : SrdMonster
    {
        [Constructable]
        public SrdBrainCoral() : base("BrainCoral") 
        {
        }

        public SrdBrainCoral(Serial serial) : base(serial) { }
    }
}
