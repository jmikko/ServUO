using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drop bear corpse")]
    public sealed class SrdDropBear : SrdMonster
    {
        [Constructable]
        public SrdDropBear() : base("DropBear") 
        {
        }

        public SrdDropBear(Serial serial) : base(serial) { }
    }
}
