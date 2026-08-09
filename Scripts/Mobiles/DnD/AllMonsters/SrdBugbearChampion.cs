using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bugbear champion corpse")]
    public sealed class SrdBugbearChampion : SrdMonster
    {
        [Constructable]
        public SrdBugbearChampion() : base("BugbearChampion") 
        {
        }

        public SrdBugbearChampion(Serial serial) : base(serial) { }
    }
}
