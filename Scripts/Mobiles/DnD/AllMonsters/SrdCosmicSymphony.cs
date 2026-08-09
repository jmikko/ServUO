using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cosmic symphony corpse")]
    public sealed class SrdCosmicSymphony : SrdMonster
    {
        [Constructable]
        public SrdCosmicSymphony() : base("CosmicSymphony") 
        {
        }

        public SrdCosmicSymphony(Serial serial) : base(serial) { }
    }
}
