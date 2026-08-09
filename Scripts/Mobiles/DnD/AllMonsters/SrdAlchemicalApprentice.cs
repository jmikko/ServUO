using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alchemical apprentice corpse")]
    public sealed class SrdAlchemicalApprentice : SrdMonster
    {
        [Constructable]
        public SrdAlchemicalApprentice() : base("AlchemicalApprentice") 
        {
        }

        public SrdAlchemicalApprentice(Serial serial) : base(serial) { }
    }
}
