using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bugbear chief corpse")]
    public sealed class SrdBugbearChief : SrdMonster
    {
        [Constructable]
        public SrdBugbearChief() : base("BugbearChief") 
        {
        }

        public SrdBugbearChief(Serial serial) : base(serial) { }
    }
}
