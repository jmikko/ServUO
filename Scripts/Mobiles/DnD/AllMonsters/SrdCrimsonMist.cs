using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crimson mist corpse")]
    public sealed class SrdCrimsonMist : SrdMonster
    {
        [Constructable]
        public SrdCrimsonMist() : base("CrimsonMist") 
        {
        }

        public SrdCrimsonMist(Serial serial) : base(serial) { }
    }
}
