using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deathspeaker corpse")]
    public sealed class SrdDeathspeaker : SrdMonster
    {
        [Constructable]
        public SrdDeathspeaker() : base("Deathspeaker") 
        {
        }

        public SrdDeathspeaker(Serial serial) : base(serial) { }
    }
}
