using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bonespitter corpse")]
    public sealed class SrdBonespitter : SrdMonster
    {
        [Constructable]
        public SrdBonespitter() : base("Bonespitter") 
        {
        }

        public SrdBonespitter(Serial serial) : base(serial) { }
    }
}
