using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a climbing vine corpse")]
    public sealed class SrdClimbingVine : SrdMonster
    {
        [Constructable]
        public SrdClimbingVine() : base("ClimbingVine") 
        {
        }

        public SrdClimbingVine(Serial serial) : base(serial) { }
    }
}
