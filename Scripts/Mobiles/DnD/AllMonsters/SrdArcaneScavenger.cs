using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a arcane scavenger corpse")]
    public sealed class SrdArcaneScavenger : SrdMonster
    {
        [Constructable]
        public SrdArcaneScavenger() : base("ArcaneScavenger") 
        {
        }

        public SrdArcaneScavenger(Serial serial) : base(serial) { }
    }
}
