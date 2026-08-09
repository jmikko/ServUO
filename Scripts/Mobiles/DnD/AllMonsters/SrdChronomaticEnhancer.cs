using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chronomatic enhancer corpse")]
    public sealed class SrdChronomaticEnhancer : SrdMonster
    {
        [Constructable]
        public SrdChronomaticEnhancer() : base("ChronomaticEnhancer") 
        {
        }

        public SrdChronomaticEnhancer(Serial serial) : base(serial) { }
    }
}
