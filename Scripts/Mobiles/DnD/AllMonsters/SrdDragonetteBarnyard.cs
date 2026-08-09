using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragonette, barnyard corpse")]
    public sealed class SrdDragonetteBarnyard : SrdMonster
    {
        [Constructable]
        public SrdDragonetteBarnyard() : base("DragonetteBarnyard") 
        {
        }

        public SrdDragonetteBarnyard(Serial serial) : base(serial) { }
    }
}
