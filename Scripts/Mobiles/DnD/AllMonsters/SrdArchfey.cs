using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a archfey corpse")]
    public sealed class SrdArchfey : SrdMonster
    {
        [Constructable]
        public SrdArchfey() : base("Archfey") 
        {
        }

        public SrdArchfey(Serial serial) : base(serial) { }
    }
}
