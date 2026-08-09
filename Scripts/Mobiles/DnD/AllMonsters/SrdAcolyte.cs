using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a acolyte corpse")]
    public sealed class SrdAcolyte : SrdMonster
    {
        [Constructable]
        public SrdAcolyte() : base("Acolyte") 
        {
        }

        public SrdAcolyte(Serial serial) : base(serial) { }
    }
}
