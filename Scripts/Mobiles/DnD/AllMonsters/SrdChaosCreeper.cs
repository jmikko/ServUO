using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chaos creeper corpse")]
    public sealed class SrdChaosCreeper : SrdMonster
    {
        [Constructable]
        public SrdChaosCreeper() : base("ChaosCreeper") 
        {
        }

        public SrdChaosCreeper(Serial serial) : base(serial) { }
    }
}
