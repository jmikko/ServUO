using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bonepowder ghoul corpse")]
    public sealed class SrdBonepowderGhoul : SrdMonster
    {
        [Constructable]
        public SrdBonepowderGhoul() : base("BonepowderGhoul") 
        {
        }

        public SrdBonepowderGhoul(Serial serial) : base(serial) { }
    }
}
