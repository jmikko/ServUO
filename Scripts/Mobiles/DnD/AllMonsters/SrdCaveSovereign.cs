using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cave sovereign corpse")]
    public sealed class SrdCaveSovereign : SrdMonster
    {
        [Constructable]
        public SrdCaveSovereign() : base("CaveSovereign") 
        {
        }

        public SrdCaveSovereign(Serial serial) : base(serial) { }
    }
}
