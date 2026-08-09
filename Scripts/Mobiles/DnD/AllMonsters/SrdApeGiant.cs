using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ape, giant corpse")]
    public sealed class SrdApeGiant : SrdMonster
    {
        [Constructable]
        public SrdApeGiant() : base("ApeGiant") 
        {
        }

        public SrdApeGiant(Serial serial) : base(serial) { }
    }
}
