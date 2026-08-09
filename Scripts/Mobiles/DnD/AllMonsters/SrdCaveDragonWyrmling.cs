using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cave dragon wyrmling corpse")]
    public sealed class SrdCaveDragonWyrmling : SrdMonster
    {
        [Constructable]
        public SrdCaveDragonWyrmling() : base("CaveDragonWyrmling") 
        {
        }

        public SrdCaveDragonWyrmling(Serial serial) : base(serial) { }
    }
}
