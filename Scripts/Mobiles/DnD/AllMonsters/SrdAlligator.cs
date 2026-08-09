using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a alligator corpse")]
    public sealed class SrdAlligator : SrdMonster
    {
        [Constructable]
        public SrdAlligator() : base("Alligator") 
        {
        }

        public SrdAlligator(Serial serial) : base(serial) { }
    }
}
