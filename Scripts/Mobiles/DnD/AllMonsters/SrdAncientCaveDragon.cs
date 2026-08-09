using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient cave dragon corpse")]
    public sealed class SrdAncientCaveDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientCaveDragon() : base("AncientCaveDragon") 
        {
        }

        public SrdAncientCaveDragon(Serial serial) : base(serial) { }
    }
}
