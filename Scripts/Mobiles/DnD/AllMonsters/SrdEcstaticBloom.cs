using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ecstatic bloom corpse")]
    public sealed class SrdEcstaticBloom : SrdMonster
    {
        [Constructable]
        public SrdEcstaticBloom() : base("EcstaticBloom") 
        {
        }

        public SrdEcstaticBloom(Serial serial) : base(serial) { }
    }
}
