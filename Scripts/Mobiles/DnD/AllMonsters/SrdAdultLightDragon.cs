using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult light dragon corpse")]
    public sealed class SrdAdultLightDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultLightDragon() : base("AdultLightDragon") 
        {
        }

        public SrdAdultLightDragon(Serial serial) : base(serial) { }
    }
}
