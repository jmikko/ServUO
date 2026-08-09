using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a archon, ursan corpse")]
    public sealed class SrdArchonUrsan : SrdMonster
    {
        [Constructable]
        public SrdArchonUrsan() : base("ArchonUrsan") 
        {
        }

        public SrdArchonUrsan(Serial serial) : base(serial) { }
    }
}
