using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alseid grovekeeper corpse")]
    public sealed class SrdAlseidGrovekeeper : SrdMonster
    {
        [Constructable]
        public SrdAlseidGrovekeeper() : base("AlseidGrovekeeper") 
        {
        }

        public SrdAlseidGrovekeeper(Serial serial) : base(serial) { }
    }
}
