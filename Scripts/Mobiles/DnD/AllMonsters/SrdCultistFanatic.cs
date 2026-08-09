using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cultist, fanatic corpse")]
    public sealed class SrdCultistFanatic : SrdMonster
    {
        [Constructable]
        public SrdCultistFanatic() : base("CultistFanatic") 
        {
        }

        public SrdCultistFanatic(Serial serial) : base(serial) { }
    }
}
