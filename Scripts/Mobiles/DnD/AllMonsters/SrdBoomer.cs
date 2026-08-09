using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a boomer corpse")]
    public sealed class SrdBoomer : SrdMonster
    {
        [Constructable]
        public SrdBoomer() : base("Boomer") 
        {
        }

        public SrdBoomer(Serial serial) : base(serial) { }
    }
}
