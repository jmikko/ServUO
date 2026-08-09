using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chained angel corpse")]
    public sealed class SrdChainedAngel : SrdMonster
    {
        [Constructable]
        public SrdChainedAngel() : base("ChainedAngel") 
        {
        }

        public SrdChainedAngel(Serial serial) : base(serial) { }
    }
}
