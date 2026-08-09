using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a earth dragon wyrmling corpse")]
    public sealed class SrdEarthDragonWyrmling : SrdMonster
    {
        [Constructable]
        public SrdEarthDragonWyrmling() : base("EarthDragonWyrmling") 
        {
        }

        public SrdEarthDragonWyrmling(Serial serial) : base(serial) { }
    }
}
