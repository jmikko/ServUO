using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chain devil corpse")]
    public sealed class SrdChainDevil : SrdMonster
    {
        [Constructable]
        public SrdChainDevil() : base("ChainDevil") 
        {
        }

        public SrdChainDevil(Serial serial) : base(serial) { }
    }
}
