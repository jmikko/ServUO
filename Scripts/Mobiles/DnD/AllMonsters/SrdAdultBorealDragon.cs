using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult boreal dragon corpse")]
    public sealed class SrdAdultBorealDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultBorealDragon() : base("AdultBorealDragon") 
        {
        }

        public SrdAdultBorealDragon(Serial serial) : base(serial) { }
    }
}
