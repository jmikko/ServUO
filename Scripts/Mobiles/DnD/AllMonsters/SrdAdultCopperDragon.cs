using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult copper dragon corpse")]
    public sealed class SrdAdultCopperDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultCopperDragon() : base("AdultCopperDragon") 
        {
        }

        public SrdAdultCopperDragon(Serial serial) : base(serial) { }
    }
}
