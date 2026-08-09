using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bolt-thrower corpse")]
    public sealed class SrdBoltThrower : SrdMonster
    {
        [Constructable]
        public SrdBoltThrower() : base("BoltThrower") 
        {
        }

        public SrdBoltThrower(Serial serial) : base(serial) { }
    }
}
