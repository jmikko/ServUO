using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult mithral dragon corpse")]
    public sealed class SrdAdultMithralDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultMithralDragon() : base("AdultMithralDragon") 
        {
        }

        public SrdAdultMithralDragon(Serial serial) : base(serial) { }
    }
}
