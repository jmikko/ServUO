using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragonette, shovel corpse")]
    public sealed class SrdDragonetteShovel : SrdMonster
    {
        [Constructable]
        public SrdDragonetteShovel() : base("DragonetteShovel") 
        {
        }

        public SrdDragonetteShovel(Serial serial) : base(serial) { }
    }
}
