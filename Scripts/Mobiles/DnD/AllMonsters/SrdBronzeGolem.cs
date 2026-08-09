using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bronze golem corpse")]
    public sealed class SrdBronzeGolem : SrdMonster
    {
        [Constructable]
        public SrdBronzeGolem() : base("BronzeGolem") 
        {
        }

        public SrdBronzeGolem(Serial serial) : base(serial) { }
    }
}
