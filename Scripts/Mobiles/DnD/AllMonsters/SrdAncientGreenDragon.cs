using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient green dragon corpse")]
    public sealed class SrdAncientGreenDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientGreenDragon() : base("AncientGreenDragon") 
        {
        }

        public SrdAncientGreenDragon(Serial serial) : base(serial) { }
    }
}
