using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a catscratch corpse")]
    public sealed class SrdCatscratch : SrdMonster
    {
        [Constructable]
        public SrdCatscratch() : base("Catscratch") 
        {
        }

        public SrdCatscratch(Serial serial) : base(serial) { }
    }
}
