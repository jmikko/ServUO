using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cherufe corpse")]
    public sealed class SrdCherufe : SrdMonster
    {
        [Constructable]
        public SrdCherufe() : base("Cherufe") 
        {
        }

        public SrdCherufe(Serial serial) : base(serial) { }
    }
}
