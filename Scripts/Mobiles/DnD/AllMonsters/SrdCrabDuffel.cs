using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crab, duffel corpse")]
    public sealed class SrdCrabDuffel : SrdMonster
    {
        [Constructable]
        public SrdCrabDuffel() : base("CrabDuffel") 
        {
        }

        public SrdCrabDuffel(Serial serial) : base(serial) { }
    }
}
