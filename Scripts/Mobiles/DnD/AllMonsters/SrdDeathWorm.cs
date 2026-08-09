using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a death worm corpse")]
    public sealed class SrdDeathWorm : SrdMonster
    {
        [Constructable]
        public SrdDeathWorm() : base("DeathWorm") 
        {
        }

        public SrdDeathWorm(Serial serial) : base(serial) { }
    }
}
