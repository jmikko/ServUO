using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient void dragon corpse")]
    public sealed class SrdAncientVoidDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientVoidDragon() : base("AncientVoidDragon") 
        {
        }

        public SrdAncientVoidDragon(Serial serial) : base(serial) { }
    }
}
