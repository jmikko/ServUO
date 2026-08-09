using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devil bough corpse")]
    public sealed class SrdDevilBough : SrdMonster
    {
        [Constructable]
        public SrdDevilBough() : base("DevilBough") 
        {
        }

        public SrdDevilBough(Serial serial) : base(serial) { }
    }
}
