using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clockwork armadillo corpse")]
    public sealed class SrdClockworkArmadillo : SrdMonster
    {
        [Constructable]
        public SrdClockworkArmadillo() : base("ClockworkArmadillo") 
        {
        }

        public SrdClockworkArmadillo(Serial serial) : base(serial) { }
    }
}
