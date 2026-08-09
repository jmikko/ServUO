using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ahu-nixta cataphract corpse")]
    public sealed class SrdAhuNixtaCataphract : SrdMonster
    {
        [Constructable]
        public SrdAhuNixtaCataphract() : base("AhuNixtaCataphract") 
        {
        }

        public SrdAhuNixtaCataphract(Serial serial) : base(serial) { }
    }
}
