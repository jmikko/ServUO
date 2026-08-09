using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dust goblin corpse")]
    public sealed class SrdDustGoblin : SrdMonster
    {
        [Constructable]
        public SrdDustGoblin() : base("DustGoblin") 
        {
        }

        public SrdDustGoblin(Serial serial) : base(serial) { }
    }
}
