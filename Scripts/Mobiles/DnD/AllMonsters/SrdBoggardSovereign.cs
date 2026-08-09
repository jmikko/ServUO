using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a boggard sovereign corpse")]
    public sealed class SrdBoggardSovereign : SrdMonster
    {
        [Constructable]
        public SrdBoggardSovereign() : base("BoggardSovereign") 
        {
        }

        public SrdBoggardSovereign(Serial serial) : base(serial) { }
    }
}
