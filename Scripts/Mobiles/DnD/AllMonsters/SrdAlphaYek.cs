using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alpha yek corpse")]
    public sealed class SrdAlphaYek : SrdMonster
    {
        [Constructable]
        public SrdAlphaYek() : base("AlphaYek") 
        {
        }

        public SrdAlphaYek(Serial serial) : base(serial) { }
    }
}
