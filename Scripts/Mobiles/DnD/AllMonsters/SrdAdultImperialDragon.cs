using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult imperial dragon corpse")]
    public sealed class SrdAdultImperialDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultImperialDragon() : base("AdultImperialDragon") 
        {
        }

        public SrdAdultImperialDragon(Serial serial) : base(serial) { }
    }
}
