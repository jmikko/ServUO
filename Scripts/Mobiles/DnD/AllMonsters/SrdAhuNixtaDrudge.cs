using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ahu-nixta drudge corpse")]
    public sealed class SrdAhuNixtaDrudge : SrdMonster
    {
        [Constructable]
        public SrdAhuNixtaDrudge() : base("AhuNixtaDrudge") 
        {
        }

        public SrdAhuNixtaDrudge(Serial serial) : base(serial) { }
    }
}
