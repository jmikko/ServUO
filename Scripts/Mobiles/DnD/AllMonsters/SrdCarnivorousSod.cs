using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a carnivorous sod corpse")]
    public sealed class SrdCarnivorousSod : SrdMonster
    {
        [Constructable]
        public SrdCarnivorousSod() : base("CarnivorousSod") 
        {
        }

        public SrdCarnivorousSod(Serial serial) : base(serial) { }
    }
}
