using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult flame dragon corpse")]
    public sealed class SrdAdultFlameDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultFlameDragon() : base("AdultFlameDragon") 
        {
        }

        public SrdAdultFlameDragon(Serial serial) : base(serial) { }
    }
}
