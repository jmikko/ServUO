using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a drakon corpse")]
    public sealed class SrdDrakon : SrdMonster
    {
        [Constructable]
        public SrdDrakon() : base("Drakon") 
        {
        }

        public SrdDrakon(Serial serial) : base(serial) { }
    }
}
