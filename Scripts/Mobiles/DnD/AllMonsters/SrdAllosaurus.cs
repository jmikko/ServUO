using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a allosaurus corpse")]
    public sealed class SrdAllosaurus : SrdMonster
    {
        [Constructable]
        public SrdAllosaurus() : base("Allosaurus") 
        {
        }

        public SrdAllosaurus(Serial serial) : base(serial) { }
    }
}
