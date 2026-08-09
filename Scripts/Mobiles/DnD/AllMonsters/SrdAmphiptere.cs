using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a amphiptere corpse")]
    public sealed class SrdAmphiptere : SrdMonster
    {
        [Constructable]
        public SrdAmphiptere() : base("Amphiptere") 
        {
        }

        public SrdAmphiptere(Serial serial) : base(serial) { }
    }
}
