using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult wind dragon corpse")]
    public sealed class SrdAdultWindDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultWindDragon() : base("AdultWindDragon") 
        {
        }

        public SrdAdultWindDragon(Serial serial) : base(serial) { }
    }
}
