using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient mithral dragon corpse")]
    public sealed class SrdAncientMithralDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientMithralDragon() : base("AncientMithralDragon") 
        {
        }

        public SrdAncientMithralDragon(Serial serial) : base(serial) { }
    }
}
