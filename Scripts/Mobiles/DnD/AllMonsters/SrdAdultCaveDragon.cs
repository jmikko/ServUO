using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult cave dragon corpse")]
    public sealed class SrdAdultCaveDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultCaveDragon() : base("AdultCaveDragon") 
        {
        }

        public SrdAdultCaveDragon(Serial serial) : base(serial) { }
    }
}
