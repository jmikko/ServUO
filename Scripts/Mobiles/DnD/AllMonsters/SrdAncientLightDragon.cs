using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient light dragon corpse")]
    public sealed class SrdAncientLightDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientLightDragon() : base("AncientLightDragon") 
        {
        }

        public SrdAncientLightDragon(Serial serial) : base(serial) { }
    }
}
