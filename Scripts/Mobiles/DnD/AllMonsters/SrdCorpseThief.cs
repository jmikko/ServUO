using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corpse thief corpse")]
    public sealed class SrdCorpseThief : SrdMonster
    {
        [Constructable]
        public SrdCorpseThief() : base("CorpseThief") 
        {
        }

        public SrdCorpseThief(Serial serial) : base(serial) { }
    }
}
