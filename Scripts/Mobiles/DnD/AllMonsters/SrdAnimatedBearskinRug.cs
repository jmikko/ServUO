using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a animated bearskin rug corpse")]
    public sealed class SrdAnimatedBearskinRug : SrdMonster
    {
        [Constructable]
        public SrdAnimatedBearskinRug() : base("AnimatedBearskinRug") 
        {
        }

        public SrdAnimatedBearskinRug(Serial serial) : base(serial) { }
    }
}
