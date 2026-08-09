using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a avestruzii corpse")]
    public sealed class SrdAvestruzii : SrdMonster
    {
        [Constructable]
        public SrdAvestruzii() : base("Avestruzii") 
        {
        }

        public SrdAvestruzii(Serial serial) : base(serial) { }
    }
}
