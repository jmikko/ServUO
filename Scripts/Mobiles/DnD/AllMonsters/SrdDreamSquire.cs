using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dream squire corpse")]
    public sealed class SrdDreamSquire : SrdMonster
    {
        [Constructable]
        public SrdDreamSquire() : base("DreamSquire") 
        {
        }

        public SrdDreamSquire(Serial serial) : base(serial) { }
    }
}
