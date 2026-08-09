using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cueyatl warrior corpse")]
    public sealed class SrdCueyatlWarrior : SrdMonster
    {
        [Constructable]
        public SrdCueyatlWarrior() : base("CueyatlWarrior") 
        {
        }

        public SrdCueyatlWarrior(Serial serial) : base(serial) { }
    }
}
