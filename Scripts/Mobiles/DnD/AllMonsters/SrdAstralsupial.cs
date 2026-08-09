using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a astralsupial corpse")]
    public sealed class SrdAstralsupial : SrdMonster
    {
        [Constructable]
        public SrdAstralsupial() : base("Astralsupial") 
        {
        }

        public SrdAstralsupial(Serial serial) : base(serial) { }
    }
}
