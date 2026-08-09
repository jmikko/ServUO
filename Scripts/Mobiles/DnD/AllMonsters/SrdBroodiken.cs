using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a broodiken corpse")]
    public sealed class SrdBroodiken : SrdMonster
    {
        [Constructable]
        public SrdBroodiken() : base("Broodiken") 
        {
        }

        public SrdBroodiken(Serial serial) : base(serial) { }
    }
}
