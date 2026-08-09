using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a djinni noble corpse")]
    public sealed class SrdDjinniNoble : SrdMonster
    {
        [Constructable]
        public SrdDjinniNoble() : base("DjinniNoble") 
        {
        }

        public SrdDjinniNoble(Serial serial) : base(serial) { }
    }
}
