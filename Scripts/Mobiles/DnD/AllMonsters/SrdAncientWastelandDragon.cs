using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient wasteland dragon corpse")]
    public sealed class SrdAncientWastelandDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientWastelandDragon() : base("AncientWastelandDragon") 
        {
        }

        public SrdAncientWastelandDragon(Serial serial) : base(serial) { }
    }
}
