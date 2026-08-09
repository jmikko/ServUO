using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult rime worm corpse")]
    public sealed class SrdAdultRimeWorm : SrdMonster
    {
        [Constructable]
        public SrdAdultRimeWorm() : base("AdultRimeWorm") 
        {
        }

        public SrdAdultRimeWorm(Serial serial) : base(serial) { }
    }
}
