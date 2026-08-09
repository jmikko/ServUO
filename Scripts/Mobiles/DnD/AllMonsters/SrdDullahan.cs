using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dullahan corpse")]
    public sealed class SrdDullahan : SrdMonster
    {
        [Constructable]
        public SrdDullahan() : base("Dullahan") 
        {
        }

        public SrdDullahan(Serial serial) : base(serial) { }
    }
}
