using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dvarapala corpse")]
    public sealed class SrdDvarapala : SrdMonster
    {
        [Constructable]
        public SrdDvarapala() : base("Dvarapala") 
        {
        }

        public SrdDvarapala(Serial serial) : base(serial) { }
    }
}
