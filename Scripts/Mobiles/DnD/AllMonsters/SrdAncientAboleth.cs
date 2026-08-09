using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient aboleth corpse")]
    public sealed class SrdAncientAboleth : SrdMonster
    {
        [Constructable]
        public SrdAncientAboleth() : base("AncientAboleth") 
        {
        }

        public SrdAncientAboleth(Serial serial) : base(serial) { }
    }
}
