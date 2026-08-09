using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cave goat corpse")]
    public sealed class SrdCaveGoat : SrdMonster
    {
        [Constructable]
        public SrdCaveGoat() : base("CaveGoat") 
        {
        }

        public SrdCaveGoat(Serial serial) : base(serial) { }
    }
}
