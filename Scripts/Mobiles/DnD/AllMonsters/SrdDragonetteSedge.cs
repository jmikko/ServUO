using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dragonette, sedge corpse")]
    public sealed class SrdDragonetteSedge : SrdMonster
    {
        [Constructable]
        public SrdDragonetteSedge() : base("DragonetteSedge") 
        {
        }

        public SrdDragonetteSedge(Serial serial) : base(serial) { }
    }
}
