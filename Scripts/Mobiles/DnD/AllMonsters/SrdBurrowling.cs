using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a burrowling corpse")]
    public sealed class SrdBurrowling : SrdMonster
    {
        [Constructable]
        public SrdBurrowling() : base("Burrowling") 
        {
        }

        public SrdBurrowling(Serial serial) : base(serial) { }
    }
}
