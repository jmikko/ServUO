using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ahu-nixta corpse")]
    public sealed class SrdAhuNixta : SrdMonster
    {
        [Constructable]
        public SrdAhuNixta() : base("AhuNixta") 
        {
        }

        public SrdAhuNixta(Serial serial) : base(serial) { }
    }
}
