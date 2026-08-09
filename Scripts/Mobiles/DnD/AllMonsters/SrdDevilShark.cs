using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devil shark corpse")]
    public sealed class SrdDevilShark : SrdMonster
    {
        [Constructable]
        public SrdDevilShark() : base("DevilShark") 
        {
        }

        public SrdDevilShark(Serial serial) : base(serial) { }
    }
}
