using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a anzu corpse")]
    public sealed class SrdAnzu : SrdMonster
    {
        [Constructable]
        public SrdAnzu() : base("Anzu") 
        {
        }

        public SrdAnzu(Serial serial) : base(serial) { }
    }
}
