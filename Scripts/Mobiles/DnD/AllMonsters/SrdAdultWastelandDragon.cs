using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a adult wasteland dragon corpse")]
    public sealed class SrdAdultWastelandDragon : SrdMonster
    {
        [Constructable]
        public SrdAdultWastelandDragon() : base("AdultWastelandDragon") 
        {
        }

        public SrdAdultWastelandDragon(Serial serial) : base(serial) { }
    }
}
