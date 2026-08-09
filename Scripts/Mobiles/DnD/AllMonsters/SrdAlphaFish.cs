using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alpha fish corpse")]
    public sealed class SrdAlphaFish : SrdMonster
    {
        [Constructable]
        public SrdAlphaFish() : base("AlphaFish") 
        {
        }

        public SrdAlphaFish(Serial serial) : base(serial) { }
    }
}
