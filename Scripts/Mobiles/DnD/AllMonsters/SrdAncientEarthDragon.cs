using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient earth dragon corpse")]
    public sealed class SrdAncientEarthDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientEarthDragon() : base("AncientEarthDragon") 
        {
        }

        public SrdAncientEarthDragon(Serial serial) : base(serial) { }
    }
}
