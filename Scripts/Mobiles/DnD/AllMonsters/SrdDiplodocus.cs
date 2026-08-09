using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a diplodocus corpse")]
    public sealed class SrdDiplodocus : SrdMonster
    {
        [Constructable]
        public SrdDiplodocus() : base("Diplodocus") 
        {
        }

        public SrdDiplodocus(Serial serial) : base(serial) { }
    }
}
