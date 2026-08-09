using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a brimstone locusthound corpse")]
    public sealed class SrdBrimstoneLocusthound : SrdMonster
    {
        [Constructable]
        public SrdBrimstoneLocusthound() : base("BrimstoneLocusthound") 
        {
        }

        public SrdBrimstoneLocusthound(Serial serial) : base(serial) { }
    }
}
