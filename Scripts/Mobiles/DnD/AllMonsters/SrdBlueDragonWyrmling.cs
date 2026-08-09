using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blue dragon wyrmling corpse")]
    public sealed class SrdBlueDragonWyrmling : SrdMonster
    {
        [Constructable]
        public SrdBlueDragonWyrmling() : base("BlueDragonWyrmling") 
        {
        }

        public SrdBlueDragonWyrmling(Serial serial) : base(serial) { }
    }
}
