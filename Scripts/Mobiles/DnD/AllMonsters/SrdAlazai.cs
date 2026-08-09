using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alazai corpse")]
    public sealed class SrdAlazai : SrdMonster
    {
        [Constructable]
        public SrdAlazai() : base("Alazai") 
        {
        }

        public SrdAlazai(Serial serial) : base(serial) { }
    }
}
