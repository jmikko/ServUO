using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crimson jelly corpse")]
    public sealed class SrdCrimsonJelly : SrdMonster
    {
        [Constructable]
        public SrdCrimsonJelly() : base("CrimsonJelly") 
        {
        }

        public SrdCrimsonJelly(Serial serial) : base(serial) { }
    }
}
