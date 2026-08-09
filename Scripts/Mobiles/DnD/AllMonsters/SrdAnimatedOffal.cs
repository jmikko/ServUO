using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a animated offal corpse")]
    public sealed class SrdAnimatedOffal : SrdMonster
    {
        [Constructable]
        public SrdAnimatedOffal() : base("AnimatedOffal") 
        {
        }

        public SrdAnimatedOffal(Serial serial) : base(serial) { }
    }
}
