using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a archon, siege corpse")]
    public sealed class SrdArchonSiege : SrdMonster
    {
        [Constructable]
        public SrdArchonSiege() : base("ArchonSiege") 
        {
        }

        public SrdArchonSiege(Serial serial) : base(serial) { }
    }
}
