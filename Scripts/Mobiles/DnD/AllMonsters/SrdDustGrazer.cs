using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dust grazer corpse")]
    public sealed class SrdDustGrazer : SrdMonster
    {
        [Constructable]
        public SrdDustGrazer() : base("DustGrazer") 
        {
        }

        public SrdDustGrazer(Serial serial) : base(serial) { }
    }
}
