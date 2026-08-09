using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cultist corpse")]
    public sealed class SrdCultist : SrdMonster
    {
        [Constructable]
        public SrdCultist() : base("Cultist") 
        {
        }

        public SrdCultist(Serial serial) : base(serial) { }
    }
}
