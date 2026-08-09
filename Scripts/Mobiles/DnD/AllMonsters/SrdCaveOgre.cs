using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cave ogre corpse")]
    public sealed class SrdCaveOgre : SrdMonster
    {
        [Constructable]
        public SrdCaveOgre() : base("CaveOgre") 
        {
        }

        public SrdCaveOgre(Serial serial) : base(serial) { }
    }
}
