using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bronze dragon wyrmling corpse")]
    public sealed class SrdBronzeDragonWyrmling : SrdMonster
    {
        [Constructable]
        public SrdBronzeDragonWyrmling() : base("BronzeDragonWyrmling") 
        {
        }

        public SrdBronzeDragonWyrmling(Serial serial) : base(serial) { }
    }
}
