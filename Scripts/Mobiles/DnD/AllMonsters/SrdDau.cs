using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dau corpse")]
    public sealed class SrdDau : SrdMonster
    {
        [Constructable]
        public SrdDau() : base("Dau") 
        {
        }

        public SrdDau(Serial serial) : base(serial) { }
    }
}
