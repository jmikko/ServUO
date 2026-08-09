using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a derro explorer corpse")]
    public sealed class SrdDerroExplorer : SrdMonster
    {
        [Constructable]
        public SrdDerroExplorer() : base("DerroExplorer") 
        {
        }

        public SrdDerroExplorer(Serial serial) : base(serial) { }
    }
}
