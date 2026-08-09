using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alchemical golem corpse")]
    public sealed class SrdAlchemicalGolem : SrdMonster
    {
        [Constructable]
        public SrdAlchemicalGolem() : base("AlchemicalGolem") 
        {
        }

        public SrdAlchemicalGolem(Serial serial) : base(serial) { }
    }
}
