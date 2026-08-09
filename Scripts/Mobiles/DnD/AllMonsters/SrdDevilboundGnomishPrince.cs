using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devilbound gnomish prince corpse")]
    public sealed class SrdDevilboundGnomishPrince : SrdMonster
    {
        [Constructable]
        public SrdDevilboundGnomishPrince() : base("DevilboundGnomishPrince") 
        {
        }

        public SrdDevilboundGnomishPrince(Serial serial) : base(serial) { }
    }
}
