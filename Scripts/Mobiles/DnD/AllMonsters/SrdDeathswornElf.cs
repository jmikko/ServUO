using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deathsworn elf corpse")]
    public sealed class SrdDeathswornElf : SrdMonster
    {
        [Constructable]
        public SrdDeathswornElf() : base("DeathswornElf") 
        {
        }

        public SrdDeathswornElf(Serial serial) : base(serial) { }
    }
}
