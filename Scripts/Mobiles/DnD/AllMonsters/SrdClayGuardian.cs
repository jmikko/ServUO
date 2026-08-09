using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clay guardian corpse")]
    public sealed class SrdClayGuardian : SrdMonster
    {
        [Constructable]
        public SrdClayGuardian() : base("ClayGuardian") 
        {
        }

        public SrdClayGuardian(Serial serial) : base(serial) { }
    }
}
