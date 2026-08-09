using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dust mephit corpse")]
    public sealed class SrdDustMephit : SrdMonster
    {
        [Constructable]
        public SrdDustMephit() : base("DustMephit") 
        {
        }

        public SrdDustMephit(Serial serial) : base(serial) { }
    }
}
