using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult amethyst dragon corpse")]
    public sealed class SrdAdultAmethystDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultAmethystDragon() : base("AdultAmethystDragon") 
        {
        }

        public SrdAdultAmethystDragon(Serial serial) : base(serial) { }
    }
}
