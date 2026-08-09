using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient imperial dragon corpse")]
    public sealed class SrdAncientImperialDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientImperialDragon() : base("AncientImperialDragon") 
        {
        }

        public SrdAncientImperialDragon(Serial serial) : base(serial) { }
    }
}
