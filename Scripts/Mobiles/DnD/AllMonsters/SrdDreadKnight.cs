using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dread knight corpse")]
    public sealed class SrdDreadKnight : SrdMonster
    {
        [Constructable]
        public SrdDreadKnight() : base("DreadKnight") 
        {
        }

        public SrdDreadKnight(Serial serial) : base(serial) { }
    }
}
