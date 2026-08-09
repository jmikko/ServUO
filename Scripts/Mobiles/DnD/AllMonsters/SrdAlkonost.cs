using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alkonost corpse")]
    public sealed class SrdAlkonost : SrdMonster
    {
        [Constructable]
        public SrdAlkonost() : base("Alkonost") 
        {
        }

        public SrdAlkonost(Serial serial) : base(serial) { }
    }
}
