using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a apaxrusl corpse")]
    public sealed class SrdApaxrusl : SrdMonster
    {
        [Constructable]
        public SrdApaxrusl() : base("Apaxrusl") 
        {
        }

        public SrdApaxrusl(Serial serial) : base(serial) { }
    }
}
