using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alseid, woad warrior corpse")]
    public sealed class SrdAlseidWoadWarrior : SrdMonster
    {
        [Constructable]
        public SrdAlseidWoadWarrior() : base("AlseidWoadWarrior") 
        {
        }

        public SrdAlseidWoadWarrior(Serial serial) : base(serial) { }
    }
}
