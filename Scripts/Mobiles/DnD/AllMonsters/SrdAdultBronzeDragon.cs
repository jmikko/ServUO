using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult bronze dragon corpse")]
    public sealed class SrdAdultBronzeDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultBronzeDragon() : base("AdultBronzeDragon") 
        {
        }

        public SrdAdultBronzeDragon(Serial serial) : base(serial) { }
    }
}
