using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a death dog corpse")]
    public sealed class SrdDeathDog : SrdMonster
    {
        [Constructable]
        public SrdDeathDog() : base("DeathDog") 
        {
        }

        public SrdDeathDog(Serial serial) : base(serial) { }
    }
}
