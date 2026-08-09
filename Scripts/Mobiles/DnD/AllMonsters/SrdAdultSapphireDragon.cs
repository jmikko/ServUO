using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult sapphire dragon corpse")]
    public sealed class SrdAdultSapphireDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultSapphireDragon() : base("AdultSapphireDragon") 
        {
        }

        public SrdAdultSapphireDragon(Serial serial) : base(serial) { }
    }
}
