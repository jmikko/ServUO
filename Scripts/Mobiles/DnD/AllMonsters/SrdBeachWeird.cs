using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a beach weird corpse")]
    public sealed class SrdBeachWeird : SrdMonster
    {
        [Constructable]
        public SrdBeachWeird() : base("BeachWeird") 
        {
        }

        public SrdBeachWeird(Serial serial) : base(serial) { }
    }
}
