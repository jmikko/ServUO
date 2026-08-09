using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a copper dragon wyrmling corpse")]
    public sealed class SrdCopperDragonWyrmling : SrdMonster
    {
        [Constructable]
        public SrdCopperDragonWyrmling() : base("CopperDragonWyrmling") 
        {
        }

        public SrdCopperDragonWyrmling(Serial serial) : base(serial) { }
    }
}
