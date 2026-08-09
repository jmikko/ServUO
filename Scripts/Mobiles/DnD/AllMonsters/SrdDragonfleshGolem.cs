using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragonflesh golem corpse")]
    public sealed class SrdDragonfleshGolem : SrdMonster
    {
        [Constructable]
        public SrdDragonfleshGolem() : base("DragonfleshGolem") 
        {
        }

        public SrdDragonfleshGolem(Serial serial) : base(serial) { }
    }
}
