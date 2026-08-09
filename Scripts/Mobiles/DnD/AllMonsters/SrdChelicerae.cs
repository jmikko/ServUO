using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chelicerae corpse")]
    public sealed class SrdChelicerae : SrdMonster
    {
        [Constructable]
        public SrdChelicerae() : base("Chelicerae") 
        {
        }

        public SrdChelicerae(Serial serial) : base(serial) { }
    }
}
