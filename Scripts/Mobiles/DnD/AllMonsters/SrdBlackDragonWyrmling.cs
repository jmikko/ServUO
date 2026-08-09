using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a black dragon wyrmling corpse")]
    public sealed class SrdBlackDragonWyrmling : SrdMonster
    {
        [Constructable]
        public SrdBlackDragonWyrmling() : base("BlackDragonWyrmling") 
        {
        }

        public SrdBlackDragonWyrmling(Serial serial) : base(serial) { }
    }
}
