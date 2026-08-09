using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient bronze dragon corpse")]
    public sealed class SrdAncientBronzeDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientBronzeDragon() : base("AncientBronzeDragon") 
        {
        }

        public SrdAncientBronzeDragon(Serial serial) : base(serial) { }
    }
}
