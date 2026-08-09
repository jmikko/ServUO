using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult shadow dragon corpse")]
    public sealed class SrdAdultShadowDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultShadowDragon() : base("AdultShadowDragon") 
        {
        }

        public SrdAdultShadowDragon(Serial serial) : base(serial) { }
    }
}
