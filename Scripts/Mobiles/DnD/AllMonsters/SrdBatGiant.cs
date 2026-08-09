using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bat, giant corpse")]
    public sealed class SrdBatGiant : SrdMonster
    {
        [Constructable]
        public SrdBatGiant() : base("BatGiant") 
        {
        }

        public SrdBatGiant(Serial serial) : base(serial) { }
    }
}
