using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult gold dragon corpse")]
    public sealed class SrdAdultGoldDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultGoldDragon() : base("AdultGoldDragon") 
        {
        }

        public SrdAdultGoldDragon(Serial serial) : base(serial) { }
    }
}
