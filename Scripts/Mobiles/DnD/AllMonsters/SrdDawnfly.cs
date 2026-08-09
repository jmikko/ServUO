using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dawnfly corpse")]
    public sealed class SrdDawnfly : SrdMonster
    {
        [Constructable]
        public SrdDawnfly() : base("Dawnfly") 
        {
        }

        public SrdDawnfly(Serial serial) : base(serial) { }
    }
}
