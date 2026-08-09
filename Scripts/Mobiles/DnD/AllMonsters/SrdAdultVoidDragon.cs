using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult void dragon corpse")]
    public sealed class SrdAdultVoidDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultVoidDragon() : base("AdultVoidDragon") 
        {
        }

        public SrdAdultVoidDragon(Serial serial) : base(serial) { }
    }
}
