using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient sapphire dragon corpse")]
    public sealed class SrdAncientSapphireDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientSapphireDragon() : base("AncientSapphireDragon") 
        {
        }

        public SrdAncientSapphireDragon(Serial serial) : base(serial) { }
    }
}
