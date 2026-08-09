using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a brass dragon wyrmling corpse")]
    public sealed class SrdBrassDragonWyrmling : SrdMonster
    {
        [Constructable]
        public SrdBrassDragonWyrmling() : base("BrassDragonWyrmling") 
        {
        }

        public SrdBrassDragonWyrmling(Serial serial) : base(serial) { }
    }
}
