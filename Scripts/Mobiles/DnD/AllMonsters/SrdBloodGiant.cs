using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blood giant corpse")]
    public sealed class SrdBloodGiant : SrdMonster
    {
        [Constructable]
        public SrdBloodGiant() : base("BloodGiant") 
        {
        }

        public SrdBloodGiant(Serial serial) : base(serial) { }
    }
}
