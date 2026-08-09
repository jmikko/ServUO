using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devil, rimepacted corpse")]
    public sealed class SrdDevilRimepacted : SrdMonster
    {
        [Constructable]
        public SrdDevilRimepacted() : base("DevilRimepacted") 
        {
        }

        public SrdDevilRimepacted(Serial serial) : base(serial) { }
    }
}
