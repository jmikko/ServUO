using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a acid ant corpse")]
    public sealed class SrdAcidAnt : SrdMonster
    {
        [Constructable]
        public SrdAcidAnt() : base("AcidAnt") 
        {
        }

        public SrdAcidAnt(Serial serial) : base(serial) { }
    }
}
