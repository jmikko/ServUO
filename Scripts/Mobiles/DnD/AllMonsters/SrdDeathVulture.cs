using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a death vulture corpse")]
    public sealed class SrdDeathVulture : SrdMonster
    {
        [Constructable]
        public SrdDeathVulture() : base("DeathVulture") 
        {
        }

        public SrdDeathVulture(Serial serial) : base(serial) { }
    }
}
