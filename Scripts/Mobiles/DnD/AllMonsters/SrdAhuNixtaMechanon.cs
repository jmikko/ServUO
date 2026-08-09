using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ahu-nixta mechanon corpse")]
    public sealed class SrdAhuNixtaMechanon : SrdMonster
    {
        [Constructable]
        public SrdAhuNixtaMechanon() : base("AhuNixtaMechanon") 
        {
        }

        public SrdAhuNixtaMechanon(Serial serial) : base(serial) { }
    }
}
