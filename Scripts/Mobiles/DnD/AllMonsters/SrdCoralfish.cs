using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a coralfish corpse")]
    public sealed class SrdCoralfish : SrdMonster
    {
        [Constructable]
        public SrdCoralfish() : base("Coralfish") 
        {
        }

        public SrdCoralfish(Serial serial) : base(serial) { }
    }
}
