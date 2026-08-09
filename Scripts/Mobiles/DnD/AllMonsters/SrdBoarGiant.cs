using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a boar, giant corpse")]
    public sealed class SrdBoarGiant : SrdMonster
    {
        [Constructable]
        public SrdBoarGiant() : base("BoarGiant") 
        {
        }

        public SrdBoarGiant(Serial serial) : base(serial) { }
    }
}
