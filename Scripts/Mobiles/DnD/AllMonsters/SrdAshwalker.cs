using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ashwalker corpse")]
    public sealed class SrdAshwalker : SrdMonster
    {
        [Constructable]
        public SrdAshwalker() : base("Ashwalker") 
        {
        }

        public SrdAshwalker(Serial serial) : base(serial) { }
    }
}
