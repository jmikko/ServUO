using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult earth dragon corpse")]
    public sealed class SrdAdultEarthDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultEarthDragon() : base("AdultEarthDragon") 
        {
        }

        public SrdAdultEarthDragon(Serial serial) : base(serial) { }
    }
}
