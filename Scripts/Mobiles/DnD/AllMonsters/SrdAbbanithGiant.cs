using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a abbanith giant corpse")]
    public sealed class SrdAbbanithGiant : SrdMonster
    {
        [Constructable]
        public SrdAbbanithGiant() : base("AbbanithGiant") 
        {
        }

        public SrdAbbanithGiant(Serial serial) : base(serial) { }
    }
}
