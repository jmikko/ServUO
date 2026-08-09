using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crocodile, giant corpse")]
    public sealed class SrdCrocodileGiant : SrdMonster
    {
        [Constructable]
        public SrdCrocodileGiant() : base("CrocodileGiant") 
        {
        }

        public SrdCrocodileGiant(Serial serial) : base(serial) { }
    }
}
