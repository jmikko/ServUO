using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a arx corpse")]
    public sealed class SrdArx : SrdMonster
    {
        [Constructable]
        public SrdArx() : base("Arx") 
        {
        }

        public SrdArx(Serial serial) : base(serial) { }
    }
}
