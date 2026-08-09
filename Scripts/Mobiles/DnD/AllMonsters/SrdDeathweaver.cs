using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deathweaver corpse")]
    public sealed class SrdDeathweaver : SrdMonster
    {
        [Constructable]
        public SrdDeathweaver() : base("Deathweaver") 
        {
        }

        public SrdDeathweaver(Serial serial) : base(serial) { }
    }
}
