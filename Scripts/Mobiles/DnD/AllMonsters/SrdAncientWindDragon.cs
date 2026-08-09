using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient wind dragon corpse")]
    public sealed class SrdAncientWindDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientWindDragon() : base("AncientWindDragon") 
        {
        }

        public SrdAncientWindDragon(Serial serial) : base(serial) { }
    }
}
