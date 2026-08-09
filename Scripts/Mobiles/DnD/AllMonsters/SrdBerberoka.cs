using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a berberoka corpse")]
    public sealed class SrdBerberoka : SrdMonster
    {
        [Constructable]
        public SrdBerberoka() : base("Berberoka") 
        {
        }

        public SrdBerberoka(Serial serial) : base(serial) { }
    }
}
