using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dune mimic corpse")]
    public sealed class SrdDuneMimic : SrdMonster
    {
        [Constructable]
        public SrdDuneMimic() : base("DuneMimic") 
        {
        }

        public SrdDuneMimic(Serial serial) : base(serial) { }
    }
}
