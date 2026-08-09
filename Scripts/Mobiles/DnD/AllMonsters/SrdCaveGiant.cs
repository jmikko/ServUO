using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cave giant corpse")]
    public sealed class SrdCaveGiant : SrdMonster
    {
        [Constructable]
        public SrdCaveGiant() : base("CaveGiant") 
        {
        }

        public SrdCaveGiant(Serial serial) : base(serial) { }
    }
}
