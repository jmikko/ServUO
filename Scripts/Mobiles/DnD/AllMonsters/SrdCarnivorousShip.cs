using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a carnivorous ship corpse")]
    public sealed class SrdCarnivorousShip : SrdMonster
    {
        [Constructable]
        public SrdCarnivorousShip() : base("CarnivorousShip") 
        {
        }

        public SrdCarnivorousShip(Serial serial) : base(serial) { }
    }
}
