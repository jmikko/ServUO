using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dipsa corpse")]
    public sealed class SrdDipsa : SrdMonster
    {
        [Constructable]
        public SrdDipsa() : base("Dipsa") 
        {
        }

        public SrdDipsa(Serial serial) : base(serial) { }
    }
}
