using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ancient amethyst dragon corpse")]
    public sealed class SrdAncientAmethystDragon : SrdMonster
    {
        [Constructable]
        public SrdAncientAmethystDragon() : base("AncientAmethystDragon") 
        {
        }

        public SrdAncientAmethystDragon(Serial serial) : base(serial) { }
    }
}
