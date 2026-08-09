using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cavelight moss corpse")]
    public sealed class SrdCavelightMoss : SrdMonster
    {
        [Constructable]
        public SrdCavelightMoss() : base("CavelightMoss") 
        {
        }

        public SrdCavelightMoss(Serial serial) : base(serial) { }
    }
}
