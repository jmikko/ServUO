using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a amethyst dragon wyrmling corpse")]
    public sealed class SrdAmethystDragonWyrmling : SrdMonster
    {
        [Constructable]
        public SrdAmethystDragonWyrmling() : base("AmethystDragonWyrmling") 
        {
        }

        public SrdAmethystDragonWyrmling(Serial serial) : base(serial) { }
    }
}
