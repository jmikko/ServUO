using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blood barnacle corpse")]
    public sealed class SrdBloodBarnacle : SrdMonster
    {
        [Constructable]
        public SrdBloodBarnacle() : base("BloodBarnacle") 
        {
        }

        public SrdBloodBarnacle(Serial serial) : base(serial) { }
    }
}
