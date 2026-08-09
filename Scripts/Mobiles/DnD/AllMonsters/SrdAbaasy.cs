using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a abaasy corpse")]
    public sealed class SrdAbaasy : SrdMonster
    {
        [Constructable]
        public SrdAbaasy() : base("Abaasy") 
        {
        }

        public SrdAbaasy(Serial serial) : base(serial) { }
    }
}
