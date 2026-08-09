using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a butatsch corpse")]
    public sealed class SrdButatsch : SrdMonster
    {
        [Constructable]
        public SrdButatsch() : base("Butatsch") 
        {
        }

        public SrdButatsch(Serial serial) : base(serial) { }
    }
}
