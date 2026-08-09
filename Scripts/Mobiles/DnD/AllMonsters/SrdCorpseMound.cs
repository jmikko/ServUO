using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corpse mound corpse")]
    public sealed class SrdCorpseMound : SrdMonster
    {
        [Constructable]
        public SrdCorpseMound() : base("CorpseMound") 
        {
        }

        public SrdCorpseMound(Serial serial) : base(serial) { }
    }
}
