using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a colláis corpse")]
    public sealed class SrdCollis : SrdMonster
    {
        [Constructable]
        public SrdCollis() : base("Collis") 
        {
        }

        public SrdCollis(Serial serial) : base(serial) { }
    }
}
