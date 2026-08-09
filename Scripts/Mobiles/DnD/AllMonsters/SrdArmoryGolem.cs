using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a armory golem corpse")]
    public sealed class SrdArmoryGolem : SrdMonster
    {
        [Constructable]
        public SrdArmoryGolem() : base("ArmoryGolem") 
        {
        }

        public SrdArmoryGolem(Serial serial) : base(serial) { }
    }
}
