using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a boreal dragon wyrmling corpse")]
    public sealed class SrdBorealDragonWyrmling : SrdMonster
    {
        [Constructable]
        public SrdBorealDragonWyrmling() : base("BorealDragonWyrmling") 
        {
        }

        public SrdBorealDragonWyrmling(Serial serial) : base(serial) { }
    }
}
