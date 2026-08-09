using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crystallite corpse")]
    public sealed class SrdCrystallite : SrdMonster
    {
        [Constructable]
        public SrdCrystallite() : base("Crystallite") 
        {
        }

        public SrdCrystallite(Serial serial) : base(serial) { }
    }
}
