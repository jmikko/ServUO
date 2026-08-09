using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient sea dragon corpse")]
    public sealed class SrdAncientSeaDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientSeaDragon() : base("AncientSeaDragon") 
        {
        }

        public SrdAncientSeaDragon(Serial serial) : base(serial) { }
    }
}
