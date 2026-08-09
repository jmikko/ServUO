using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a archon, word corpse")]
    public sealed class SrdArchonWord : SrdMonster
    {
        [Constructable]
        public SrdArchonWord() : base("ArchonWord") 
        {
        }

        public SrdArchonWord(Serial serial) : base(serial) { }
    }
}
