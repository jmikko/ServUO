using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dream eater corpse")]
    public sealed class SrdDreamEater : SrdMonster
    {
        [Constructable]
        public SrdDreamEater() : base("DreamEater") 
        {
        }

        public SrdDreamEater(Serial serial) : base(serial) { }
    }
}
