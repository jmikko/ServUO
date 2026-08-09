using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a diseased giant rat corpse")]
    public sealed class SrdDiseasedGiantRat : SrdMonster
    {
        [Constructable]
        public SrdDiseasedGiantRat() : base("DiseasedGiantRat") 
        {
        }

        public SrdDiseasedGiantRat(Serial serial) : base(serial) { }
    }
}
