using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragon, sand wyrmling corpse")]
    public sealed class SrdDragonSandWyrmling : SrdMonster
    {
        [Constructable]
        public SrdDragonSandWyrmling() : base("DragonSandWyrmling") 
        {
        }

        public SrdDragonSandWyrmling(Serial serial) : base(serial) { }
    }
}
