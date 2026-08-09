using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a archaeopteryx corpse")]
    public sealed class SrdArchaeopteryx : SrdMonster
    {
        [Constructable]
        public SrdArchaeopteryx() : base("Archaeopteryx") 
        {
        }

        public SrdArchaeopteryx(Serial serial) : base(serial) { }
    }
}
