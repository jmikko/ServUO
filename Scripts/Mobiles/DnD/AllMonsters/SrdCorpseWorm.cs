using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corpse worm corpse")]
    public sealed class SrdCorpseWorm : SrdMonster
    {
        [Constructable]
        public SrdCorpseWorm() : base("CorpseWorm") 
        {
        }

        public SrdCorpseWorm(Serial serial) : base(serial) { }
    }
}
