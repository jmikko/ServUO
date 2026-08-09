using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a azi dahaka corpse")]
    public sealed class SrdAziDahaka : SrdMonster
    {
        [Constructable]
        public SrdAziDahaka() : base("AziDahaka") 
        {
        }

        public SrdAziDahaka(Serial serial) : base(serial) { }
    }
}
