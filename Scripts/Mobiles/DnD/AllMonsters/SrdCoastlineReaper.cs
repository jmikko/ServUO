using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a coastline reaper corpse")]
    public sealed class SrdCoastlineReaper : SrdMonster
    {
        [Constructable]
        public SrdCoastlineReaper() : base("CoastlineReaper") 
        {
        }

        public SrdCoastlineReaper(Serial serial) : base(serial) { }
    }
}
