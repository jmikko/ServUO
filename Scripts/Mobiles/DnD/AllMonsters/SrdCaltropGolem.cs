using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a caltrop golem corpse")]
    public sealed class SrdCaltropGolem : SrdMonster
    {
        [Constructable]
        public SrdCaltropGolem() : base("CaltropGolem") 
        {
        }

        public SrdCaltropGolem(Serial serial) : base(serial) { }
    }
}
