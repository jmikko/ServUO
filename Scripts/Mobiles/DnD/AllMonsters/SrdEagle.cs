using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a eagle corpse")]
    public sealed class SrdEagle : SrdMonster
    {
        [Constructable]
        public SrdEagle() : base("Eagle") 
        {
        }

        public SrdEagle(Serial serial) : base(serial) { }
    }
}
