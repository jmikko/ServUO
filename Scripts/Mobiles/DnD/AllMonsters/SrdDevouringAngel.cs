using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devouring angel corpse")]
    public sealed class SrdDevouringAngel : SrdMonster
    {
        [Constructable]
        public SrdDevouringAngel() : base("DevouringAngel") 
        {
        }

        public SrdDevouringAngel(Serial serial) : base(serial) { }
    }
}
