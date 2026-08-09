using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bereginyas corpse")]
    public sealed class SrdBereginyas : SrdMonster
    {
        [Constructable]
        public SrdBereginyas() : base("Bereginyas") 
        {
        }

        public SrdBereginyas(Serial serial) : base(serial) { }
    }
}
