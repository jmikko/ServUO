using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a death barque corpse")]
    public sealed class SrdDeathBarque : SrdMonster
    {
        [Constructable]
        public SrdDeathBarque() : base("DeathBarque") 
        {
        }

        public SrdDeathBarque(Serial serial) : base(serial) { }
    }
}
