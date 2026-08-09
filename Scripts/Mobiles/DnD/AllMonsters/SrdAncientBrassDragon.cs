using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient brass dragon corpse")]
    public sealed class SrdAncientBrassDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientBrassDragon() : base("AncientBrassDragon") 
        {
        }

        public SrdAncientBrassDragon(Serial serial) : base(serial) { }
    }
}
