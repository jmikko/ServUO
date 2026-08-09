using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cave mimic corpse")]
    public sealed class SrdCaveMimic : SrdMonster
    {
        [Constructable]
        public SrdCaveMimic() : base("CaveMimic") 
        {
        }

        public SrdCaveMimic(Serial serial) : base(serial) { }
    }
}
