using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a doom creeper corpse")]
    public sealed class SrdDoomCreeper : SrdMonster
    {
        [Constructable]
        public SrdDoomCreeper() : base("DoomCreeper") 
        {
        }

        public SrdDoomCreeper(Serial serial) : base(serial) { }
    }
}
