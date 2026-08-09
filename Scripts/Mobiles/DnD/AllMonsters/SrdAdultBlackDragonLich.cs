using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult black dragon lich corpse")]
    public sealed class SrdAdultBlackDragonLich : SrdMonster
    {
        [Constructable]
        public SrdAdultBlackDragonLich() : base("AdultBlackDragonLich") 
        {
        }

        public SrdAdultBlackDragonLich(Serial serial) : base(serial) { }
    }
}
