using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a bleakheart corpse")]
    public sealed class SrdBleakheart : SrdMonster
    {
        [Constructable]
        public SrdBleakheart() : base("Bleakheart") 
        {
        }

        public SrdBleakheart(Serial serial) : base(serial) { }
    }
}
