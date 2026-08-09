using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a draft horse corpse")]
    public sealed class SrdDraftHorse : SrdMonster
    {
        [Constructable]
        public SrdDraftHorse() : base("DraftHorse") 
        {
        }

        public SrdDraftHorse(Serial serial) : base(serial) { }
    }
}
