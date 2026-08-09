using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cavefish zombie corpse")]
    public sealed class SrdCavefishZombie : SrdMonster
    {
        [Constructable]
        public SrdCavefishZombie() : base("CavefishZombie") 
        {
        }

        public SrdCavefishZombie(Serial serial) : base(serial) { }
    }
}
