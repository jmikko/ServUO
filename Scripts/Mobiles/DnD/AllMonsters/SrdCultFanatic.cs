using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cult fanatic corpse")]
    public sealed class SrdCultFanatic : SrdMonster
    {
        [Constructable]
        public SrdCultFanatic() : base("CultFanatic") 
        {
        }

        public SrdCultFanatic(Serial serial) : base(serial) { }
    }
}
