using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a altar flame golem corpse")]
    public sealed class SrdAltarFlameGolem : SrdMonster
    {
        [Constructable]
        public SrdAltarFlameGolem() : base("AltarFlameGolem") 
        {
        }

        public SrdAltarFlameGolem(Serial serial) : base(serial) { }
    }
}
