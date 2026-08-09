using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a eldritch ooze corpse")]
    public sealed class SrdEldritchOoze : SrdMonster
    {
        [Constructable]
        public SrdEldritchOoze() : base("EldritchOoze") 
        {
        }

        public SrdEldritchOoze(Serial serial) : base(serial) { }
    }
}
