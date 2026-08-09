using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cauldronborn corpse")]
    public sealed class SrdCauldronborn : SrdMonster
    {
        [Constructable]
        public SrdCauldronborn() : base("Cauldronborn") 
        {
        }

        public SrdCauldronborn(Serial serial) : base(serial) { }
    }
}
