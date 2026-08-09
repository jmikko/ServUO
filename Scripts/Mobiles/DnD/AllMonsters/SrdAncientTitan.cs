using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient titan corpse")]
    public sealed class SrdAncientTitan : SrdMonster
    {
        [Constructable]
        public SrdAncientTitan() : base("AncientTitan") 
        {
        }

        public SrdAncientTitan(Serial serial) : base(serial) { }
    }
}
