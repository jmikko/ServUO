using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragon cultist corpse")]
    public sealed class SrdDragonCultist : SrdMonster
    {
        [Constructable]
        public SrdDragonCultist() : base("DragonCultist") 
        {
        }

        public SrdDragonCultist(Serial serial) : base(serial) { }
    }
}
