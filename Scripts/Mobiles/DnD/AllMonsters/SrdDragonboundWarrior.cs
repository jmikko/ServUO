using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragonbound warrior corpse")]
    public sealed class SrdDragonboundWarrior : SrdMonster
    {
        [Constructable]
        public SrdDragonboundWarrior() : base("DragonboundWarrior") 
        {
        }

        public SrdDragonboundWarrior(Serial serial) : base(serial) { }
    }
}
