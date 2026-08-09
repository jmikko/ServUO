using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a arcane blademaster corpse")]
    public sealed class SrdArcaneBlademaster : SrdMonster
    {
        [Constructable]
        public SrdArcaneBlademaster() : base("ArcaneBlademaster") 
        {
        }

        public SrdArcaneBlademaster(Serial serial) : base(serial) { }
    }
}
