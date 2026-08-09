using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient red dragon corpse")]
    public sealed class SrdAncientRedDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientRedDragon() : base("AncientRedDragon") 
        {
        }

        public SrdAncientRedDragon(Serial serial) : base(serial) { }
    }
}
