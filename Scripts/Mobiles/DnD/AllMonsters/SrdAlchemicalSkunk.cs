using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alchemical skunk corpse")]
    public sealed class SrdAlchemicalSkunk : SrdMonster
    {
        [Constructable]
        public SrdAlchemicalSkunk() : base("AlchemicalSkunk") 
        {
        }

        public SrdAlchemicalSkunk(Serial serial) : base(serial) { }
    }
}
