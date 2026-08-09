using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult silver dragon corpse")]
    public sealed class SrdAdultSilverDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultSilverDragon() : base("AdultSilverDragon") 
        {
        }

        public SrdAdultSilverDragon(Serial serial) : base(serial) { }
    }
}
