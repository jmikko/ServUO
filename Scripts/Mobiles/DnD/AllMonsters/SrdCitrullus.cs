using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a citrullus corpse")]
    public sealed class SrdCitrullus : SrdMonster
    {
        [Constructable]
        public SrdCitrullus() : base("Citrullus") 
        {
        }

        public SrdCitrullus(Serial serial) : base(serial) { }
    }
}
