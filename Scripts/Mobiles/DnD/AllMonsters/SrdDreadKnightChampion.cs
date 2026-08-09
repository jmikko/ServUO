using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dread knight champion corpse")]
    public sealed class SrdDreadKnightChampion : SrdMonster
    {
        [Constructable]
        public SrdDreadKnightChampion() : base("DreadKnightChampion") 
        {
        }

        public SrdDreadKnightChampion(Serial serial) : base(serial) { }
    }
}
