using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bloated ghoul corpse")]
    public sealed class SrdBloatedGhoul : SrdMonster
    {
        [Constructable]
        public SrdBloatedGhoul() : base("BloatedGhoul") 
        {
        }

        public SrdBloatedGhoul(Serial serial) : base(serial) { }
    }
}
