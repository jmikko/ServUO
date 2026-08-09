using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drudge pitcher corpse")]
    public sealed class SrdDrudgePitcher : SrdMonster
    {
        [Constructable]
        public SrdDrudgePitcher() : base("DrudgePitcher") 
        {
        }

        public SrdDrudgePitcher(Serial serial) : base(serial) { }
    }
}
