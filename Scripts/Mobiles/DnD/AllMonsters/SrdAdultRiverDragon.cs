using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult river dragon corpse")]
    public sealed class SrdAdultRiverDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultRiverDragon() : base("AdultRiverDragon") 
        {
        }

        public SrdAdultRiverDragon(Serial serial) : base(serial) { }
    }
}
