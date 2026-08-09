using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult sea dragon corpse")]
    public sealed class SrdAdultSeaDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultSeaDragon() : base("AdultSeaDragon") 
        {
        }

        public SrdAdultSeaDragon(Serial serial) : base(serial) { }
    }
}
