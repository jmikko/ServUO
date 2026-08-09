using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bastet temple cat corpse")]
    public sealed class SrdBastetTempleCat : SrdMonster
    {
        [Constructable]
        public SrdBastetTempleCat() : base("BastetTempleCat") 
        {
        }

        public SrdBastetTempleCat(Serial serial) : base(serial) { }
    }
}
