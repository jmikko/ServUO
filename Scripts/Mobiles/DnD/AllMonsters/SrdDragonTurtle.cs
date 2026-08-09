using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragon turtle corpse")]
    public sealed class SrdDragonTurtle : SrdMonster
    {
        [Constructable]
        public SrdDragonTurtle() : base("DragonTurtle") 
        {
        }

        public SrdDragonTurtle(Serial serial) : base(serial) { }
    }
}
